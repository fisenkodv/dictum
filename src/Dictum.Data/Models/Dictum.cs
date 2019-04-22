using Dictum.Data.Extensions;
using System;

namespace Dictum.Data.Models
{
    internal class Dictum
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Quote { get; set; }
        public string Author { get; set; }
        public int LanguageId { get; set; }
        public DateTime AddedAt { get; set; }
    }

    internal static class DictumSchema
    {
        public static string Table { get; } = nameof(Dictum).ToPlural().ToSnakeCase();

        public static class Columns
        {
            public static string Uuid { get; } = nameof(Dictum.Uuid).ToSnakeCase();
            public static string Quote { get; } = nameof(Dictum.Quote).ToSnakeCase();
            public static string Author { get; } = nameof(Dictum.Author).ToSnakeCase();
            public static string LanguageId { get; } = nameof(Dictum.LanguageId).ToSnakeCase();
        }
    }
}
