using System.Threading.Tasks;
using GiftSuggestionService.Dtos;

namespace GiftSuggestionService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendGiftSuggestionToCommand(GiftSuggestionReadDto plat);
    }
}