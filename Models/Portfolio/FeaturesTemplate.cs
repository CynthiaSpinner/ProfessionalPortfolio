using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class FeaturesTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string SectionTitle { get; set; } = string.Empty;

        [StringLength(200)]
        public string SectionSubtitle { get; set; } = string.Empty;

        [StringLength(500)]
        public string SectionDescription { get; set; } = string.Empty;

        // Feature 1
        [Required]
        [StringLength(100)]
        public string Feature1Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Feature1Subtitle { get; set; } = string.Empty;

        [StringLength(500)]
        public string Feature1Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string Feature1Icon { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Feature1Link { get; set; } = string.Empty;

        // Feature 2
        [Required]
        [StringLength(100)]
        public string Feature2Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Feature2Subtitle { get; set; } = string.Empty;

        [StringLength(500)]
        public string Feature2Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string Feature2Icon { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Feature2Link { get; set; } = string.Empty;

        // Feature 3
        [Required]
        [StringLength(100)]
        public string Feature3Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Feature3Subtitle { get; set; } = string.Empty;

        [StringLength(500)]
        public string Feature3Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string Feature3Icon { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Feature3Link { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 