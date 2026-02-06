using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class CTASection
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Subtitle { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ButtonText { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string ButtonLink { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
