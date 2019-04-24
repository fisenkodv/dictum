using JetBrains.Annotations;

namespace Dictum.Business.Models
{
    [PublicAPI]
    public class Quote
    {
        public string Uuid { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
    }
}