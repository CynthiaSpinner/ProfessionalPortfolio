using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class Video
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        public string ThumbnailPath { get; set; } = string.Empty;

        [Required]
        public string FileType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public int Duration { get; set; } // Duration in seconds

        public DateTime UploadDate { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
} 