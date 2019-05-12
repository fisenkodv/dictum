using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class AuthorsService
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 0;
        private const int MaxPageSize = 50;

        private readonly IAuthorRepository _authorRepository;

        public AuthorsService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<IEnumerable<Author>> GetAuthors(string query, int? page, int? count)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query is empty", nameof(query));

            if (!page.HasValue) page = DefaultPageIndex;
            if (!count.HasValue) count = DefaultPageSize;

            if (count > MaxPageSize) throw new ArgumentException("Items per page is too high", nameof(count));

            return _authorRepository.GetAuthors(query, page.Value, count.Value);
        }
    }
}