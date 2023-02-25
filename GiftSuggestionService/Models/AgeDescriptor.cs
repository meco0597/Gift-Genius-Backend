
using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AgeDescriptor
    {
        PreTeens,
        Teens,
        Twenties,
        Thirties,
        Forties,
        Fifties,
        Sixties,
        Seventies,
        Eighties,
        Nineties,
    }
}