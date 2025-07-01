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
        
        public float HeaderOverlayOpacity { get; set; } = 0.5f;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 