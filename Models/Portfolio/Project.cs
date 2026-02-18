using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Portfolio.Models.Portfolio
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Subtitle { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>Vimeo embed URL (e.g. https://player.vimeo.com/video/123) or Vimeo video ID. Optional.</summary>
        public string? VideoUrl { get; set; }

        [Column(TypeName = "json")]
        public string FeaturesJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Features
        {
            get => JsonSerializer.Deserialize<List<string>>(FeaturesJson) ?? new List<string>();
            set => FeaturesJson = JsonSerializer.Serialize(value);
        }

        [Required]
        public string ProjectUrl { get; set; } = string.Empty;

        [Required]
        public string GithubUrl { get; set; } = string.Empty;

        [Column(TypeName = "json")]
        public string TechnologiesJson { get; set; } = "[]";

        [NotMapped]
        public List<string> Technologies
        {
            get => JsonSerializer.Deserialize<List<string>>(TechnologiesJson) ?? new List<string>();
            set => TechnologiesJson = JsonSerializer.Serialize(value);
        }

        public DateTime CompletionDate { get; set; }
    }
}