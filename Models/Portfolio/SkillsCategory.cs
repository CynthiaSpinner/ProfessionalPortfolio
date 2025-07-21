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
        [Column("Name")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;

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