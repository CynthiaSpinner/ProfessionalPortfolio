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
        public string? Feature1Description { get; set; } = "";

        [MaxLength(200)]
        public string? Feature1Link { get; set; } = "/projects?category=frontend";

        [MaxLength(100)]
        public string? Feature1LinkText { get; set; } = "Learn more";

        [Required]
        [MaxLength(100)]
        public string Feature2Title { get; set; } = "Backend Development";

        [MaxLength(200)]
        public string Feature2Subtitle { get; set; } = ".NET Core, C#, RESTful APIs, SQL Server";

        [MaxLength(500)]
        public string? Feature2Description { get; set; } = "";

        [MaxLength(200)]
        public string? Feature2Link { get; set; } = "/projects?category=backend";

        [MaxLength(100)]
        public string? Feature2LinkText { get; set; } = "Learn more";

        [Required]
        [MaxLength(100)]
        public string Feature3Title { get; set; } = "Design & Tools";

        [MaxLength(200)]
        public string Feature3Subtitle { get; set; } = "Adobe Creative Suite, UI/UX Design, Git, Docker";

        [MaxLength(500)]
        public string? Feature3Description { get; set; } = "";

        [MaxLength(200)]
        public string? Feature3Link { get; set; } = "/projects?category=design";

        [MaxLength(100)]
        public string? Feature3LinkText { get; set; } = "Learn more";

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 1;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
