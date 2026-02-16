using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models.Portfolio
{
    /// <summary>Single-row settings for the site (e.g. which nav links to show).</summary>
    public class SiteSettings
    {
        public int Id { get; set; }

        /// <summary>Show "Graphic Design" in the main nav. Turn off until the page has more content.</summary>
        public bool ShowGraphicDesignLink { get; set; } = true;

        /// <summary>Show "Design" in the main nav. Turn off until the page has more content.</summary>
        public bool ShowDesignLink { get; set; } = true;

        public DateTime? UpdatedAt { get; set; }
    }
}
