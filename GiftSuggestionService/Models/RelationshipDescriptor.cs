using System.Text.Json.Serialization;

namespace GiftSuggestionService.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RelationshipDescriptor
    {
        Friend,

        Sister,
        Brother,

        Mother,
        Father,

        Girlfriend,
        Boyfriend,

        Wife,
        Husband,

        Grandma,
        Grandpa,

        Daughter,
        Son,

        Cousin,

        Aunt,
        Uncle,
    }
}