using Dictum.Data.Extensions;
using Humanizer;
using JetBrains.Annotations;

namespace Dictum.Data.Models
{
    [UsedImplicitly]
    internal class Author
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
    }

    internal static class AuthorSchema
    {
        public static string Table { get; } = nameof(Author).Pluralize().ToSnakeCase();

        public static class Columns
        {
            public static string Id { get; } = nameof(Author.Id).ToSnakeCase();
            public static string Uuid { get; } = nameof(Author.Uuid).ToSnakeCase();
        }
    }
}