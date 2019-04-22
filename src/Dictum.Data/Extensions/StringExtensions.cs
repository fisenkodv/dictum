using Dictum.Business.Services.Pluralization;
using System.Text.RegularExpressions;

namespace Dictum.Data.Extensions
{
    internal static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string ToPlural(this string input)
        {
            var service = PluralizationService.CreateService();
            return service.IsSingular(input) ? service.Pluralize(input) : input;
        }
    }
}
