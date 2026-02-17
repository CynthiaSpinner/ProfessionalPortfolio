using Portfolio.Models.Portfolio;

namespace Portfolio.Services.DTOs;

public class SkillsPublicDto
{
    public List<SkillsCategory> Categories { get; set; } = new();
    public string LinkText { get; set; } = "View what I spin?";
}
