using JetBrains.Annotations;

namespace Dictum.Business.Models.Dto
{
    [PublicAPI]
    public class LanguageDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}