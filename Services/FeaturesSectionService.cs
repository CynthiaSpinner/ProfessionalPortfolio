using Portfolio.Data.Repositories;
using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;
using Portfolio.Services.Interfaces;

namespace Portfolio.Services;

public class FeaturesSectionService : IFeaturesSectionService
{
    private readonly IFeaturesSectionRepository _repository;

    public FeaturesSectionService(IFeaturesSectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<object?> GetAdminDtoAsync()
    {
        var section = await _repository.GetFirstOrDefaultAsync();
        if (section == null) return null;
        return new
        {
            sectionTitle = section.SectionTitle,
            sectionSubtitle = section.SectionSubtitle,
            feature1Title = section.Feature1Title,
            feature1Subtitle = section.Feature1Subtitle,
            feature1Description = section.Feature1Description,
            feature1Link = section.Feature1Link,
            feature1LinkText = section.Feature1LinkText,
            feature2Title = section.Feature2Title,
            feature2Subtitle = section.Feature2Subtitle,
            feature2Description = section.Feature2Description,
            feature2Link = section.Feature2Link,
            feature2LinkText = section.Feature2LinkText,
            feature3Title = section.Feature3Title,
            feature3Subtitle = section.Feature3Subtitle,
            feature3Description = section.Feature3Description,
            feature3Link = section.Feature3Link,
            feature3LinkText = section.Feature3LinkText,
            isActive = section.IsActive,
            displayOrder = section.DisplayOrder,
            updatedAt = section.UpdatedAt
        };
    }

    public async Task<object> GetForPublicApiAsync()
    {
        var section = await _repository.GetFirstOrDefaultAsync();
        if (section == null)
        {
            return new
            {
                sectionTitle = "Key Skills & Technologies",
                sectionSubtitle = "Explore my expertise across different domains",
                features = new[]
                {
                    new { title = "Frontend Development", subtitle = "React, JavaScript, HTML5, CSS3, Bootstrap", description = "", link = "/projects?category=frontend", linkText = "Learn more" },
                    new { title = "Backend Development", subtitle = ".NET Core, C#, RESTful APIs, SQL Server", description = "", link = "/projects?category=backend", linkText = "Learn more" },
                    new { title = "Design & Tools", subtitle = "Adobe Creative Suite, UI/UX Design, Git, Docker", description = "", link = "/projects?category=design", linkText = "Learn more" }
                },
                lastModified = DateTime.UtcNow
            };
        }
        return new
        {
            sectionTitle = section.SectionTitle,
            sectionSubtitle = section.SectionSubtitle,
            features = new[]
            {
                new { title = section.Feature1Title, subtitle = section.Feature1Subtitle, description = section.Feature1Description ?? "", link = section.Feature1Link ?? "", linkText = section.Feature1LinkText ?? "Learn more" },
                new { title = section.Feature2Title, subtitle = section.Feature2Subtitle, description = section.Feature2Description ?? "", link = section.Feature2Link ?? "", linkText = section.Feature2LinkText ?? "Learn more" },
                new { title = section.Feature3Title, subtitle = section.Feature3Subtitle, description = section.Feature3Description ?? "", link = section.Feature3Link ?? "", linkText = section.Feature3LinkText ?? "Learn more" }
            },
            lastModified = section.UpdatedAt
        };
    }

    public async Task<(bool Success, string Message)> SaveFromDtoAsync(FeaturesSectionEditDto dto)
    {
        var section = await _repository.GetFirstOrDefaultAsync();
        if (section == null)
        {
            section = new FeaturesSection();
            await _repository.AddAsync(section);
        }

        section.SectionTitle = dto.SectionTitle ?? "Key Skills & Technologies";
        section.SectionSubtitle = dto.SectionSubtitle ?? "Explore my expertise across different domains";
        section.Feature1Title = dto.Feature1Title ?? "Frontend Development";
        section.Feature1Subtitle = dto.Feature1Subtitle ?? "React, JavaScript, HTML5, CSS3, Bootstrap";
        section.Feature1Description = dto.Feature1Description ?? "";
        section.Feature1Link = dto.Feature1Link ?? "/projects?category=frontend";
        section.Feature1LinkText = dto.Feature1LinkText ?? "Learn more";
        section.Feature2Title = dto.Feature2Title ?? "Backend Development";
        section.Feature2Subtitle = dto.Feature2Subtitle ?? ".NET Core, C#, RESTful APIs, SQL Server";
        section.Feature2Description = dto.Feature2Description ?? "";
        section.Feature2Link = dto.Feature2Link ?? "/projects?category=backend";
        section.Feature2LinkText = dto.Feature2LinkText ?? "Learn more";
        section.Feature3Title = dto.Feature3Title ?? "Design & Tools";
        section.Feature3Subtitle = dto.Feature3Subtitle ?? "Adobe Creative Suite, UI/UX Design, Git, Docker";
        section.Feature3Description = dto.Feature3Description ?? "";
        section.Feature3Link = dto.Feature3Link ?? "/projects?category=design";
        section.Feature3LinkText = dto.Feature3LinkText ?? "Learn more";
        section.IsActive = dto.IsActive;
        section.DisplayOrder = dto.DisplayOrder;
        section.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(section);
        return (true, "Features section saved successfully!");
    }
}
