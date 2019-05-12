using Dictum.Data.Extensions;
using Humanizer;
using JetBrains.Annotations;

namespace Dictum.Data.Models
{
    [UsedImplicitly]
    public class AuthorName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int LanguageId { get; set; }
    }

    internal static class AuthorNameSchema
    {
        public static string Table { get; } = nameof(AuthorName).Pluralize().ToSnakeCase();

        public static class Columns
        {
            public static string Id { get; } = nameof(AuthorName.Id).ToSnakeCase();
            public static string Name { get; } = nameof(AuthorName.Name).ToSnakeCase();
            public static string AuthorId { get; } = nameof(AuthorName.AuthorId).ToSnakeCase();
            public static string LanguageId { get; } = nameof(AuthorName.LanguageId).ToSnakeCase();
        }
    }
}