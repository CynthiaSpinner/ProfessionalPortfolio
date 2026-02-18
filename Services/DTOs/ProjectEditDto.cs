namespace Portfolio.Services.DTOs;

public class ProjectEditDto
{
    public int? Id { get; set; }
    public string Title { get; set; } = "";
    public string? Subtitle { get; set; }
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string? VideoUrl { get; set; }
    public List<string> Features { get; set; } = new();
    public List<string> Technologies { get; set; } = new();
    public string ProjectUrl { get; set; } = "";
    public string GithubUrl { get; set; } = "";
    public DateTime CompletionDate { get; set; }
}
