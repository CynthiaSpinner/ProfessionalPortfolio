namespace Portfolio.Services.DTOs;

public class HeroPublicDto
{
    public string Title { get; set; } = "";
    public string Subtitle { get; set; } = "";
    public string Description { get; set; } = "";
    public string BackgroundImageUrl { get; set; } = "";
    public string? BackgroundVideoUrl { get; set; }
    public string PrimaryButtonText { get; set; } = "";
    public string PrimaryButtonUrl { get; set; } = "";
    public string OverlayColor { get; set; } = "#000000";
    public float OverlayOpacity { get; set; }
    public DateTime LastModified { get; set; }
}
