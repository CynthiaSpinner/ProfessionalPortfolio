using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Portfolio.Models.Portfolio
{
    public class SkillsCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; } = "";

        public string? ImagePath { get; set; }

        [Column(TypeName = "json")]
        public string SkillsJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Skills
        {
            get => JsonSerializer.Deserialize<List<string>>(SkillsJson) ?? new List<string>();
            set => SkillsJson = JsonSerializer.Serialize(value);
        }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}