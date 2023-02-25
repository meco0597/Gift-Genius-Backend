
using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Pronoun
    {
        Their = 0,
        His,
        Her,
    }
}