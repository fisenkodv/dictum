namespace Dictum.Business.Services.Pluralization
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pluralization")]
    public interface ICustomPluralizationMapping
    {
        void AddWord(string singular, string plural);
    }
}
