using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Portfolio.Models.Portfolio
{
    public class HomePage
    {
        [Key]
        public int Id { get; set; }

        // Header Section
        [Required]
        [StringLength(100)]
        public string HeaderTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string HeaderSubtitle { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? HeaderDescription { get; set; }

        [StringLength(500)]
        public string? HeaderBackgroundImageUrl { get; set; }

        [StringLength(500)]
        public string? HeaderBackgroundVideoUrl { get; set; }

        [StringLength(500)]
        public string? HeaderBackgroundStyle { get; set; }

        [StringLength(50)]
        public string? HeaderPrimaryButtonText { get; set; }

        [StringLength(200)]
        public string? HeaderPrimaryButtonUrl { get; set; }

        [StringLength(50)]
        public string? HeaderSecondaryButtonText { get; set; }

        [StringLength(200)]
        public string? HeaderSecondaryButtonUrl { get; set; }

        public bool ShowHeaderPrimaryButton { get; set; } = true;
        public bool ShowHeaderSecondaryButton { get; set; } = true;

        [StringLength(50)]
        public string? HeaderOverlayColor { get; set; }

        public float? HeaderOverlayOpacity { get; set; }

        [StringLength(50)]
        public string? HeaderTextColor { get; set; }

        [StringLength(50)]
        public string? HeaderButtonColor { get; set; }

        [StringLength(50)]
        public string? HeaderButtonTextColor { get; set; }

        // Skills Section
        [StringLength(100)]
        public string? SkillsSectionTitle { get; set; }

        [StringLength(500)]
        public string? SkillsSectionSubtitle { get; set; }

        [Column(TypeName = "json")]
        public string SkillsCategoriesJson { get; set; } = "[]";

        [NotMapped]
        public List<SkillsCategory> SkillsCategories
        {
            get => JsonSerializer.Deserialize<List<SkillsCategory>>(SkillsCategoriesJson) ?? new List<SkillsCategory>();
            set => SkillsCategoriesJson = JsonSerializer.Serialize(value);
        }

        // Featured Projects Section
        [StringLength(100)]
        public string? FeaturedProjectsTitle { get; set; }

        [StringLength(500)]
        public string? FeaturedProjectsSubtitle { get; set; }

        [Column(TypeName = "json")]
        public string FeaturedProjectsJson { get; set; } = "[]";

        [NotMapped]
        public List<int> FeaturedProjectIds
        {
            get => JsonSerializer.Deserialize<List<int>>(FeaturedProjectsJson) ?? new List<int>();
            set => FeaturedProjectsJson = JsonSerializer.Serialize(value);
        }

        // Call to Action Section
        [StringLength(100)]
        public string? CTATitle { get; set; }

        [StringLength(500)]
        public string? CTASubtitle { get; set; }

        [StringLength(50)]
        public string? CTAButtonText { get; set; }

        [StringLength(200)]
        public string? CTAButtonUrl { get; set; }

        [StringLength(500)]
        public string? CTABackgroundImageUrl { get; set; }

        [StringLength(50)]
        public string? CTATextColor { get; set; }

        // General Settings
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public List<Project> FeaturedProjects { get; set; } = new List<Project>();
    }
}