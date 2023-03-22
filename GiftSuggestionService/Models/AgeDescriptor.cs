
using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AgeDescriptor
    {
        // default
        Thirties = 0,

        Toddler,
        Preschool,
        Gradeschool,
        MiddleSchool,
        Teens,
        Twenties,
        Forties,
        Fifties,
        Sixties,
        Seventies,
        Eighties,
        Nineties,
    }
}