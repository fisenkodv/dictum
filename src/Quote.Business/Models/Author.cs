using JetBrains.Annotations;

namespace Dictum.Business.Models
{
    [PublicAPI]
    public class Author
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
    }
}