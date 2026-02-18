using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Project>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Project?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<(bool Success, string Message, int? Id)> SaveAsync(ProjectEditDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return (false, "Title is required.", null);

        if (dto.Id.HasValue && dto.Id.Value > 0)
        {
            var existing = await _repository.GetByIdAsync(dto.Id.Value);
            if (existing == null)
                return (false, "Project not found.", null);
            existing.Title = dto.Title.Trim();
            existing.Subtitle = string.IsNullOrWhiteSpace(dto.Subtitle) ? null : dto.Subtitle.Trim();
            existing.Description = dto.Description?.Trim() ?? "";
            existing.ImageUrl = dto.ImageUrl?.Trim() ?? "";
            existing.VideoUrl = string.IsNullOrWhiteSpace(dto.VideoUrl) ? null : NormalizeVimeoUrl(dto.VideoUrl.Trim());
            existing.Features = dto.Features ?? new List<string>();
            existing.Technologies = dto.Technologies ?? new List<string>();
            existing.ProjectUrl = dto.ProjectUrl?.Trim() ?? "";
            existing.GithubUrl = dto.GithubUrl?.Trim() ?? "";
            existing.CompletionDate = ToUtc(dto.CompletionDate ?? DateTime.UtcNow);
            await _repository.UpdateAsync(existing);
            return (true, "Project saved.", existing.Id);
        }

        var project = new Project
        {
            Title = dto.Title.Trim(),
            Subtitle = string.IsNullOrWhiteSpace(dto.Subtitle) ? null : dto.Subtitle.Trim(),
            Description = dto.Description?.Trim() ?? "",
            ImageUrl = dto.ImageUrl?.Trim() ?? "",
            VideoUrl = string.IsNullOrWhiteSpace(dto.VideoUrl) ? null : NormalizeVimeoUrl(dto.VideoUrl.Trim()),
            Features = dto.Features ?? new List<string>(),
            Technologies = dto.Technologies ?? new List<string>(),
            ProjectUrl = dto.ProjectUrl?.Trim() ?? "",
            GithubUrl = dto.GithubUrl?.Trim() ?? "",
            CompletionDate = ToUtc(dto.CompletionDate ?? DateTime.UtcNow)
        };
        project = await _repository.AddAsync(project);
        return (true, "Project saved.", project.Id);
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var project = await _repository.GetByIdAsync(id);
        if (project == null)
            return (false, "Project not found.");
        await _repository.DeleteAsync(project);
        return (true, "Project deleted.");
    }

    /// <summary>PostgreSQL timestamp with time zone requires UTC. Normalize so Kind is Utc.</summary>
    private static DateTime ToUtc(DateTime value)
    {
        if (value.Kind == DateTimeKind.Utc) return value;
        if (value.Kind == DateTimeKind.Unspecified) return DateTime.SpecifyKind(value, DateTimeKind.Utc);
        return value.ToUniversalTime();
    }

    /// <summary>Accept full Vimeo URL or bare ID; return embed URL for iframe src.</summary>
    private static string? NormalizeVimeoUrl(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        input = input.Trim();
        // Already an embed URL
        if (input.StartsWith("https://player.vimeo.com/video/", StringComparison.OrdinalIgnoreCase))
            return input;
        if (input.StartsWith("https://vimeo.com/", StringComparison.OrdinalIgnoreCase))
        {
            var id = input.Replace("https://vimeo.com/", "", StringComparison.OrdinalIgnoreCase).TrimEnd('/');
            if (int.TryParse(id, out _)) return $"https://player.vimeo.com/video/{id}";
        }
        // Bare numeric ID
        if (int.TryParse(input, out var videoId))
            return $"https://player.vimeo.com/video/{videoId}";
        return input;
    }
}
