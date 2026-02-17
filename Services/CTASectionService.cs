using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class CTASectionService : ICTASectionService
{
    private readonly ICTASectionRepository _repository;

    public CTASectionService(ICTASectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<object?> GetAdminDtoAsync()
    {
        var section = await _repository.GetFirstOrDefaultAsync();
        if (section == null) return null;
        return new
        {
            title = section.Title ?? "",
            subtitle = section.Subtitle ?? "",
            buttonText = section.ButtonText ?? "",
            buttonLink = section.ButtonLink ?? "",
            lastModified = section.UpdatedAt ?? section.CreatedAt
        };
    }

    public async Task<object> GetForPublicApiAsync()
    {
        var section = await _repository.GetFirstOrDefaultAsync();
        if (section == null)
        {
            return new
            {
                title = "Ready to Start a Project?",
                subtitle = "Let's work together to bring your ideas to life.",
                buttonText = "Get in Touch",
                buttonLink = "/contact",
                lastModified = DateTime.UtcNow
            };
        }
        return new
        {
            title = section.Title,
            subtitle = section.Subtitle,
            buttonText = section.ButtonText,
            buttonLink = section.ButtonLink,
            lastModified = section.UpdatedAt ?? section.CreatedAt
        };
    }

    public async Task<(bool Success, string Message)> SaveFromDtoAsync(CTASectionEditDto dto)
    {
        var section = await _repository.GetFirstOrDefaultAsync();
        if (section == null)
        {
            section = new CTASection
            {
                CreatedAt = DateTime.UtcNow,
                Title = dto.Title ?? "",
                Subtitle = dto.Subtitle ?? "",
                ButtonText = dto.ButtonText ?? "",
                ButtonLink = dto.ButtonLink ?? "",
                UpdatedAt = DateTime.UtcNow
            };
            await _repository.AddAsync(section);
        }
        else
        {
            section.Title = dto.Title ?? "";
            section.Subtitle = dto.Subtitle ?? "";
            section.ButtonText = dto.ButtonText ?? "";
            section.ButtonLink = dto.ButtonLink ?? "";
            section.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(section);
        }
        return (true, "CTA section saved successfully!");
    }
}
