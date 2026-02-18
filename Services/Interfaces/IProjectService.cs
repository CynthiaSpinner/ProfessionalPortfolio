using Portfolio.Models.Portfolio;
using Portfolio.Services.DTOs;

namespace Portfolio.Services.Interfaces;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<(bool Success, string Message, int? Id)> SaveAsync(ProjectEditDto dto);
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
