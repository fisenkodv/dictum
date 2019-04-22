using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Dictum.Business.Services.Pluralization
{
    internal static class PluralizationServiceUtil
    {
        internal static bool DoesWordContainSuffix(string word, IEnumerable<string> suffixes, CultureInfo culture)
        {
            return suffixes.Any(s => word.EndsWith(s, true, culture));
        }

        internal static bool TryInflectOnSuffixInWord(string word, IEnumerable<string> suffixes, Func<string, string> operationOnWord, CultureInfo culture, out string newWord)
        {
            newWord = null;

            if (DoesWordContainSuffix(word, suffixes, culture))
            {
                newWord = operationOnWord(word);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
