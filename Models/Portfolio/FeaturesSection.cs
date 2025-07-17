using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class FeaturesSection
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string SectionTitle { get; set; } = "Key Skills & Technologies";
        
        [MaxLength(100)]
        public string Feature1Title { get; set; } = "Frontend Development";
        
        [MaxLength(200)]
        public string Feature1Subtitle { get; set; } = "React, JavaScript, HTML5, CSS3, Bootstrap";
        
        [MaxLength(100)]
        public string Feature2Title { get; set; } = "Backend Development";
        
        [MaxLength(200)]
        public string Feature2Subtitle { get; set; } = ".NET Core, C#, RESTful APIs, MySQL";
        
        [MaxLength(100)]
        public string Feature3Title { get; set; } = "Design & Tools";
        
        [MaxLength(200)]
        public string Feature3Subtitle { get; set; } = "Adobe Creative Suite, UI/UX Design, Git, Docker";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 