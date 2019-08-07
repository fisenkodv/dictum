using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dictum.Business.Services;
using Dictum.Data.Repositories;
using Kurukuru;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Quote.Importer
{
    internal static class Program
    {
        private const bool SplitIntoChunks = false;
        private const int ChunkSize = 8;

        private static async Task Main(string[] args)
        {
            var configuration = CreateConfiguration();
            var languageService = new LanguageService(new LanguageRepository(configuration));
            var authorService = new AuthorService(languageService, new AuthorRepository(configuration));
            var quoteService = new QuoteService(authorService, languageService, new QuoteRepository(configuration));

            var quotesDirectory = configuration["QuotesDirectory"];
            var quoteFiles = Directory
                .GetFiles(quotesDirectory, "*.json")
                .OrderBy(Path.GetFileNameWithoutExtension);

            foreach (var quoteFile in quoteFiles)
            {
                await Spinner.StartAsync($"Importing: {Path.GetFileName(quoteFile)}",
                    () => CreateQuotes(quoteFile, authorService, quoteService, SplitIntoChunks));
                var newQuoteFilePath = Path.Combine(quotesDirectory, "processed", Path.GetFileName(quoteFile));
                File.Move(quoteFile, newQuoteFilePath);
            }
        }

        private static IConfiguration CreateConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", optional: false);
            return configurationBuilder.Build();
        }

        private static async Task CreateQuotes(
            string filePath,
            AuthorService authorService,
            QuoteService quoteService,
            bool splitIntoChunks)
        {
            var quotes = JsonConvert.DeserializeObject<Dictum.Business.Models.Internal.Quote[]>(File.ReadAllText(filePath));
            var authors = quotes.Select(x => x.Author).Distinct();
            await Task.WhenAll(authors.Select(authorService.GetOrCreate));
            if (splitIntoChunks)
            {
                var chunks = quotes
                    .Select((quote, index) => new {quote, index})
                    .GroupBy(x => x.index / ChunkSize)
                    .Select(x => x.Select(y => y.quote));

                foreach (var chunk in chunks)
                {
                    await Task.WhenAll(chunk.Select(quoteService.CreateQuote));
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            else
                await Task.WhenAll(quotes.Select(quoteService.CreateQuote));
        }
    }
}