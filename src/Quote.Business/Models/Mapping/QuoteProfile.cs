using AutoMapper;
using Dictum.Business.Models.Domain;
using Dictum.Business.Models.Dto;
using JetBrains.Annotations;

namespace Dictum.Business.Models.Mapping
{
    [UsedImplicitly]
    internal class QuoteProfile : Profile
    {
        public QuoteProfile()
        {
            CreateMap<Quote, QuoteDto>()
                .ReverseMap();
        }
    }
}