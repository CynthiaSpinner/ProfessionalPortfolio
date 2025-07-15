using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class HeroTemplate
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nickname { get; set; } = "";
        
        [StringLength(200)]
        public string HeaderTitle { get; set; } = "";
        
        [StringLength(500)]
        public string HeaderSubtitle { get; set; } = "";
        
        public string HeaderDescription { get; set; } = "";
        
        [StringLength(500)]
        public string HeaderBackgroundImageUrl { get; set; } = "";
        
        [StringLength(500)]
        public string HeaderBackgroundVideoUrl { get; set; } = "";
        
        [StringLength(100)]
        public string HeaderPrimaryButtonText { get; set; } = "";
        
        [StringLength(200)]
        public string HeaderPrimaryButtonUrl { get; set; } = "";
        
        [StringLength(7)]
        public string HeaderOverlayColor { get; set; } = "#000000";
        
        public float? HeaderOverlayOpacity { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // CTA (Call to Action) properties
        [StringLength(500)]
        public string? CTABackgroundImageUrl { get; set; }
        
        [StringLength(100)]
        public string? CTAButtonText { get; set; }
        
        [StringLength(500)]
        public string? CTAButtonUrl { get; set; }
        
        [StringLength(200)]
        public string? CTASubtitle { get; set; }
        
        [StringLength(20)]
        public string? CTATextColor { get; set; }
        
        [StringLength(200)]
        public string? CTATitle { get; set; }
        
        // Featured Projects properties
        public string? FeaturedProjectsJson { get; set; }
        
        [StringLength(200)]
        public string? FeaturedProjectsSubtitle { get; set; }
        
        [StringLength(200)]
        public string? FeaturedProjectsTitle { get; set; }
        
        // Header styling properties
        [StringLength(50)]
        public string? HeaderBackgroundStyle { get; set; }
        
        [StringLength(20)]
        public string? HeaderButtonColor { get; set; }
        
        [StringLength(20)]
        public string? HeaderButtonTextColor { get; set; }
        
        [StringLength(100)]
        public string? HeaderSecondaryButtonText { get; set; }
        
        [StringLength(500)]
        public string? HeaderSecondaryButtonUrl { get; set; }
        
        [StringLength(20)]
        public string? HeaderTextColor { get; set; }
        
        public bool? ShowHeaderPrimaryButton { get; set; }
        
        public bool? ShowHeaderSecondaryButton { get; set; }
        
        // Skills section properties
        public string? SkillsCategoriesJson { get; set; }
        
        [StringLength(200)]
        public string? SkillsSectionSubtitle { get; set; }
        
        [StringLength(200)]
        public string? SkillsSectionTitle { get; set; }
        
        // Additional properties
        [StringLength(100)]
        public string Name { get; set; } = "";
        
        [StringLength(500)]
        public string Template { get; set; } = "";
        
        public bool? IsActive { get; set; }
    }
} 