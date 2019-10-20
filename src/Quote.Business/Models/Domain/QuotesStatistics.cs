using System.Collections.Generic;

namespace Dictum.Business.Models.Domain
{
    public class QuotesStatistics
    {
        public int Total { get; set; }
        public Dictionary<string, int> ByLanguage { get; set; }
    }

    public class AuthorStatistics
    {
        public int Total { get; set; }
        public Dictionary<string, int> ByLanguage { get; set; }
    }

    public class Statistics
    {
        public AuthorStatistics Authors { get; set; }
        public QuotesStatistics Quotes { get; set; }
    }
}