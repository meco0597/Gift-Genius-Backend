using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RelationshipDescriptor
    {
        Friend,

        Partner,

        Sibling,

        Parent,

        Child,

        Grandparent,

        Relative,

        Colleague,

        Acquaintance,
    }
}