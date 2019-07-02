using JetBrains.Annotations;

namespace Dictum.Business.Models
{
    [PublicAPI]
    public class Quote
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
    }
}