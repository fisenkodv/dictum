using JetBrains.Annotations;

namespace Dictum.Business.Models
{
    [PublicAPI]
    public class Author
    {
        public int Id { get; set; }
        public string Uuid { get; set; }
        public string Name { get; set; }
    }
}