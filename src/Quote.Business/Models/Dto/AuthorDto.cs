using JetBrains.Annotations;

namespace Dictum.Business.Models.Dto
{
    [PublicAPI]
    public class AuthorDto
    {
        public string Uuid { get; set; }
        public string Name { get; set; }
    }
}