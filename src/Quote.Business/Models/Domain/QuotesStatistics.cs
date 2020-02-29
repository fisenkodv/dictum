using System.Collections.Generic;

namespace Dictum.Business.Models.Domain
{
    public class QuotesStatistics
    {
        public int Total { get; set; }
        public Dictionary<string, int> Languages { get; set; } = new Dictionary<string, int>();
    }

    public class AuthorStatistics
    {
        public int Total { get; set; }
        public Dictionary<string, int> Languages { get; set; } = new Dictionary<string, int>();
    }

    public class Statistics
    {
        public AuthorStatistics Authors { get; set; } = new AuthorStatistics();
        public QuotesStatistics Quotes { get; set; } = new QuotesStatistics();
    }
}