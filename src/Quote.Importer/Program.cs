using System;
using System.IO;
using System.Threading.Tasks;
using Dictum.Business.Services;
using Dictum.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Quote.Importer
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = CreateConfiguration();
            var languageService = new LanguageService(new LanguageRepository(configuration));
            var authorService = new AuthorService(languageService, new AuthorRepository(configuration));
            var quoteService = new QuoteService(authorService, languageService, new QuoteRepository(configuration));

            var quoteFiles = Directory.GetFiles(configuration["QuotesDirectory"], "*.json");

            foreach (var quoteFile in quoteFiles)
            {
                await CreateQuote(quoteFile, quoteService);
            }
        }

        private static IConfiguration CreateConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true);
            return configurationBuilder.Build();
        }

        private static async Task CreateQuote(string filePath, QuoteService quoteService)
        {
            var quotes = JsonConvert.DeserializeObject<Dictum.Business.Models.Quote[]>(File.ReadAllText(filePath));
            foreach (var quote in quotes)
            {
                var createdQuote = await quoteService.CreateQuote(quote);
                Console.WriteLine($"New quote has been created with id: {createdQuote.Uuid}");
            }
        }
    }
}