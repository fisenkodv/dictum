using Dictum.Data.Extensions;
using Humanizer;
using JetBrains.Annotations;

namespace Dictum.Data.Models
{
    [UsedImplicitly]
    internal class Language
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    internal static class LanguageSchema
    {
        public static string Table { get; } = nameof(Language).Pluralize().ToSnakeCase();

        public static class Columns
        {
            public static string Id { get; } = nameof(Language.Id).ToSnakeCase();
            public static string Code { get; } = nameof(Language.Code).ToSnakeCase();
            public static string Name { get; } = nameof(Language.Name).ToSnakeCase();
        }
    }
}