using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dictum.Business.Abstract.Repositories;
using Dictum.Business.Models;

namespace Dictum.Business.Services
{
    public class AuthorService
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 0;
        private const int MaxPageSize = 50;

        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<IEnumerable<Author>> GetAuthors(string query, int? page, int? count)
        {
            if (!page.HasValue || page < 0) page = DefaultPageIndex;
            if (!count.HasValue || count <= 0) count = DefaultPageSize;

            if (count > MaxPageSize) throw new ArgumentException("Items per page is too high", nameof(count));

            return _authorRepository.GetAuthors(query, page.Value, count.Value);
        }
    }
}