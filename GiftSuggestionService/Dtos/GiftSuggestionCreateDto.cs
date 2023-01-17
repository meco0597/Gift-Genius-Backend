using System.ComponentModel.DataAnnotations;

namespace GiftSuggestionService.Dtos
{
    public class GiftSuggestionCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Cost { get; set; }
    }
}