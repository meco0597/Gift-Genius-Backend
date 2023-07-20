using System.Collections.Generic;
using System.Text.Json.Serialization;
using GiftSuggestionService.Models;

namespace GiftSuggestionService.Dtos
{
    public class GiftSuggestionSearchDto
    {
        public Pronoun Pronoun { get; set; }

        public string Occasion { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public List<string> AssociatedInterests { get; set; }

        public RelationshipDescriptor AssociatedRelationship { get; set; }

        public AgeDescriptor AssociatedAge { get; set; }
    }
}