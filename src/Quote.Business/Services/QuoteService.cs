﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models.Domain;

namespace Dictum.Business.Services
{
    public class QuoteService
    {
        private readonly AuthorService _authorService;
        private readonly LanguageService _languageService;
        private readonly IQuoteRepository _quoteRepository;

        public QuoteService(
            AuthorService authorService,
            LanguageService languageService,
            IQuoteRepository quoteRepository)
        {
            _authorService = authorService;
            _languageService = languageService;
            _quoteRepository = quoteRepository;
        }

        public Task<Quote> GetRandomQuote(string? languageCode)
        {
            languageCode = string.IsNullOrWhiteSpace(languageCode)
                ? LanguageService.DefaultLanguageCode
                : languageCode;

            return _quoteRepository.GetRandom(languageCode);
        }

        public Task<Quote> GetQuoteById(string uuid)
        {
            return _quoteRepository.GetById(uuid);
        }

        public Task<IEnumerable<Quote>> GetAuthorQuotes(string? languageCode, string authorUuid, int? page, int? count)
        {
            var paging = Paging.Create(page, count);
            languageCode = string.IsNullOrWhiteSpace(languageCode)
                ? LanguageService.DefaultLanguageCode
                : languageCode;

            return _quoteRepository.GetByAuthor(languageCode, authorUuid, paging.Page, paging.Count);
        }

        public async Task<Quote?> CreateQuote(Quote quote)
        {
            var author = await _authorService.GetOrCreate(quote.Author);
            var language = await _languageService.Detect(quote.Text);
            return await _quoteRepository.Create(quote, author, language);
        }
    }
}