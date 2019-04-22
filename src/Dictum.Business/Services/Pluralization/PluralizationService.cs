using System.Globalization;

namespace Dictum.Business.Services.Pluralization
{
    /// <summary>
    /// From https://github.com/Microsoft/referencesource/tree/master/System.Data.Entity.Design/System/Data/Entity/Design/PluralizationService
    /// </summary>
    public abstract class PluralizationService
    {
        public CultureInfo Culture { get; protected set; }

        public abstract bool IsPlural(string word);
        public abstract bool IsSingular(string word);
        public abstract string Pluralize(string word);
        public abstract string Singularize(string word);

        public static PluralizationService CreateService()
        {
            return new EnglishPluralizationService();
        }
    }
}
