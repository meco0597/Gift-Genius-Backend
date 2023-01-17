using AutoMapper;
using GiftSuggestionService.Dtos;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Profiles
{
    public class GiftSuggestionsProfile : Profile
    {
        public GiftSuggestionsProfile()
        {
            // Source -> Target
            CreateMap<GiftSuggestion, GiftSuggestionReadDto>();
            CreateMap<GiftSuggestionCreateDto, GiftSuggestion>();
            CreateMap<GiftSuggestionReadDto, GiftSuggestionPublishedDto>();
        }
    }
}