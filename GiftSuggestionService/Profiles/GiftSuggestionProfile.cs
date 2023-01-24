using AutoMapper;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Models;
using GiftSuggestionService.Utilities;

namespace GiftSuggestionService.Profiles
{
    public class GiftSuggestionsProfile : Profile
    {
        public GiftSuggestionsProfile()
        {
            // Source -> Target
            CreateMap<GiftSuggestion, GiftSuggestionReadDto>();
            CreateMap<GiftSuggestionReadDto, GiftSuggestion>();
            CreateMap<GiftSuggestion, GiftSuggestionPutDto>();
            CreateMap<GiftSuggestionPutDto, GiftSuggestion>()
                .ForMember(dest => dest.Id, act => act.MapFrom(src => GiftSuggestionUtilities.GenerateGiftSuggestionIdFromName(src.GiftName)));
        }
    }
}