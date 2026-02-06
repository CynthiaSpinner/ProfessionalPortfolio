using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class FeaturesSection
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string SectionTitle { get; set; } = "Key Skills & Technologies";

        [MaxLength(500)]
        public string? SectionSubtitle { get; set; } = "Explore my expertise across different domains";

        [Required]
        [MaxLength(100)]
        public string Feature1Title { get; set; } = "Frontend Development";

        [MaxLength(200)]
        public string Feature1Subtitle { get; set; } = "React, JavaScript, HTML5, CSS3, Bootstrap";

        [MaxLength(500)]
        public string? Feature1Description { get; set; } = "Building responsive and interactive user interfaces with modern frameworks and best practices.";

        [MaxLength(100)]
        public string? Feature1Icon { get; set; } = "fas fa-code";

        [MaxLength(200)]
        public string? Feature1Link { get; set; } = "/projects?category=frontend";

        [Required]
        [MaxLength(100)]
        public string Feature2Title { get; set; } = "Backend Development";

        [MaxLength(200)]
        public string Feature2Subtitle { get; set; } = ".NET Core, C#, RESTful APIs, SQL Server";

        [MaxLength(500)]
        public string? Feature2Description { get; set; } = "Creating robust server-side applications and APIs with enterprise-grade technologies.";

        [MaxLength(100)]
        public string? Feature2Icon { get; set; } = "fas fa-server";

        [MaxLength(200)]
        public string? Feature2Link { get; set; } = "/projects?category=backend";

        [Required]
        [MaxLength(100)]
        public string Feature3Title { get; set; } = "Design & Tools";

        [MaxLength(200)]
        public string Feature3Subtitle { get; set; } = "Adobe Creative Suite, UI/UX Design, Git, Docker";

        [MaxLength(500)]
        public string? Feature3Description { get; set; } = "Crafting beautiful designs and managing development workflows with professional tools.";

        [MaxLength(100)]
        public string? Feature3Icon { get; set; } = "fas fa-palette";

        [MaxLength(200)]
        public string? Feature3Link { get; set; } = "/projects?category=design";

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
