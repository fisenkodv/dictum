using System;

namespace Dictum.Business.Services
{
    internal class Paging
    {
        private const int DefaultPageSize = 10;
        private const int DefaultPageIndex = 0;
        private const int MaxPageSize = 50;

        private Paging(int page, int count)
        {
            Page = page;
            Count = count;
        }

        public int Page { get; }
        public int Count { get; }

        public static Paging Create(int? page, int? count)
        {
            if (!page.HasValue || page < 0) page = DefaultPageIndex;
            if (!count.HasValue || count <= 0) count = DefaultPageSize;

            if (count > MaxPageSize) throw new ArgumentException("Items per page is too high", nameof(count));

            return new Paging(page.Value, count.Value);
        }
    }
}