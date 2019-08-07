using JetBrains.Annotations;

namespace Dictum.Business.Models.Dto
{
    [PublicAPI]
    public class QuoteDto
    {
        public string Uuid { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
    }
}