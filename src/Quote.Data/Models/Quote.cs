using System;
using Dictum.Data.Extensions;
using Humanizer;
using JetBrains.Annotations;

namespace Dictum.Data.Models
{
    [UsedImplicitly]
    internal class Quote
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Text { get; set; }
        public string Hash { get; set; }
        public int AuthorId { get; set; }
        public int LanguageId { get; set; }
        public DateTime AddedAt { get; set; }
    }

    internal static class QuoteSchema
    {
        public static string Table { get; } = nameof(Quote).Pluralize().ToSnakeCase();

        public static class Columns
        {
            public static string Id { get; } = nameof(Quote.Id).ToSnakeCase();
            public static string Uuid { get; } = nameof(Quote.Uuid).ToSnakeCase();
            public static string Text { get; } = nameof(Quote.Text).ToSnakeCase();
            public static string Hash { get; } = nameof(Quote.Hash).ToSnakeCase();
            public static string AuthorId { get; } = nameof(Quote.AuthorId).ToSnakeCase();
            public static string LanguageId { get; } = nameof(Quote.LanguageId).ToSnakeCase();
            public static string AddedAt { get; } = nameof(Quote.AddedAt).ToSnakeCase();
        }
    }
}