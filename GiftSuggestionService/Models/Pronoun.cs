
using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Pronoun
    {
        They = 0,
        He,
        She,
    }
}