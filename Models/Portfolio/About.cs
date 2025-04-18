using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Portfolio.Models.Portfolio
{
    public class About
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Biography { get; set; } = string.Empty;

        [Required]
        public string ProfileImageUrl { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string LinkedInUrl { get; set; } = string.Empty;

        [Required]
        public string GithubUrl { get; set; } = string.Empty;

        [Column(TypeName = "json")]
        public string InterestsJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Interests
        {
            get => JsonSerializer.Deserialize<List<string>>(InterestsJson) ?? new List<string>();
            set => InterestsJson = JsonSerializer.Serialize(value);
        }

        public List<WorkExperience> WorkExperience { get; set; } = new List<WorkExperience>();

        public List<Education> Education { get; set; } = new List<Education>();
    }

    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Company { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "json")]
        public string AchievementsJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Achievements
        {
            get => JsonSerializer.Deserialize<List<string>>(AchievementsJson) ?? new List<string>();
            set => AchievementsJson = JsonSerializer.Serialize(value);
        }
    }

    public class Education
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Institution { get; set; } = string.Empty;

        [Required]
        public string Degree { get; set; } = string.Empty;

        [Required]
        public string Field { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}