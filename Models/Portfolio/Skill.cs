using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public int Proficiency { get; set; }

        [Required]
        public string IconUrl { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}