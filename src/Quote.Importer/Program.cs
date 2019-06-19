﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dictum.Business.Services;
using Dictum.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Quote.Importer
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var configuration = CreateConfiguration();
            var languageService = new LanguageService(new LanguageRepository(configuration));
            var authorService = new AuthorService(languageService, new AuthorRepository(configuration));
            var quoteService = new QuoteService(authorService, languageService, new QuoteRepository(configuration));

            var quoteFiles = Directory.GetFiles(configuration["QuotesDirectory"], "*.json");

            foreach (var quoteFile in quoteFiles)
            {
                Console.WriteLine($"Importing: {Path.GetFileName(quoteFile)}");
                await CreateQuotes(quoteFile, quoteService);
            }
        }

        private static IConfiguration CreateConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", optional: false);
            return configurationBuilder.Build();
        }

        private static async Task CreateQuotes(string filePath, QuoteService quoteService)
        {
            var quotes = JsonConvert.DeserializeObject<Dictum.Business.Models.Quote[]>(File.ReadAllText(filePath));
            var createQuoteTasks = quotes.Select(quoteService.CreateQuote);
            await Task.WhenAll(createQuoteTasks);
        }
    }
}