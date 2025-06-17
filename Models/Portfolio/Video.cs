using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    public class Video
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string ThumbnailPath { get; set; }

        [Required]
        public string FileType { get; set; }

        public long FileSize { get; set; }

        public int Duration { get; set; } // Duration in seconds

        public DateTime UploadDate { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
} 