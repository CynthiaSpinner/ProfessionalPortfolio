using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    /// <summary>Single-row config for the CTA block on the Projects page.</summary>
    public class ProjectsPageCTA
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "Ready to work together?";

        [StringLength(500)]
        public string Subtitle { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ButtonText { get; set; } = "Get in Touch";

        [Required]
        [StringLength(500)]
        public string ButtonLink { get; set; } = "/contact";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
