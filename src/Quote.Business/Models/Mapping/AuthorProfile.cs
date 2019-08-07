using AutoMapper;
using Dictum.Business.Models.Dto;
using Dictum.Business.Models.Internal;
using JetBrains.Annotations;

namespace Dictum.Business.Models.Mapping
{
    [UsedImplicitly]
    internal class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ReverseMap();
        }
    }
}