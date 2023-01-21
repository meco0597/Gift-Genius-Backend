using GiftSuggestionService.Dtos;

namespace GiftSuggestionService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewGiftSuggestion(GiftSuggestionPutDto giftSuggestionPublishedDto);
    }
}