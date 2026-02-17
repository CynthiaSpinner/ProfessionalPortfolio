namespace Portfolio.Services.DTOs;

/// <summary>Input for updating the Features section (admin form).</summary>
public class FeaturesSectionEditDto
{
    public string? SectionTitle { get; set; }
    public string? SectionSubtitle { get; set; }
    public string? Feature1Title { get; set; }
    public string? Feature1Subtitle { get; set; }
    public string? Feature1Description { get; set; }
    public string? Feature1Link { get; set; }
    public string? Feature1LinkText { get; set; }
    public string? Feature2Title { get; set; }
    public string? Feature2Subtitle { get; set; }
    public string? Feature2Description { get; set; }
    public string? Feature2Link { get; set; }
    public string? Feature2LinkText { get; set; }
    public string? Feature3Title { get; set; }
    public string? Feature3Subtitle { get; set; }
    public string? Feature3Description { get; set; }
    public string? Feature3Link { get; set; }
    public string? Feature3LinkText { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; } = 1;
}
