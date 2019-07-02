using JetBrains.Annotations;

namespace Dictum.Business.Models
{
    [PublicAPI]
    public class Language
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}