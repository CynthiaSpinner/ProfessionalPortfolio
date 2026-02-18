using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    /// <summary>Single-row config for the Projects page hero (title, subtitle, button). Background texture is CSS on frontend.</summary>
    public class ProjectsPageHero
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "My Projects";

        [StringLength(500)]
        public string? Subtitle { get; set; }

        [StringLength(100)]
        public string? ButtonText { get; set; }

        [StringLength(500)]
        public string? ButtonUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
