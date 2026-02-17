using Portfolio.Services.DTOs;

namespace Portfolio.Services.Interfaces;

public interface ICTASectionService
{
    Task<object?> GetAdminDtoAsync();
    Task<object> GetForPublicApiAsync();
    Task<(bool Success, string Message)> SaveFromDtoAsync(CTASectionEditDto dto);
}
