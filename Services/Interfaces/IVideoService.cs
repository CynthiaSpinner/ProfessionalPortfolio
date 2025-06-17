using Microsoft.AspNetCore.Http;
using Portfolio.Models.Portfolio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Services.Interfaces
{
    public interface IVideoService
    {
        Task<Video> UploadVideoAsync(IFormFile videoFile, string title, string description);
        Task<bool> DeleteVideoAsync(int videoId);
        Task<Video> GetVideoByIdAsync(int videoId);
        Task<IEnumerable<Video>> GetAllVideosAsync();
        Task<IEnumerable<Video>> GetActiveVideosAsync();
        Task<bool> UpdateVideoAsync(Video video);
        Task<bool> UpdateVideoStatusAsync(int videoId, bool isActive);
        Task<bool> UpdateVideoOrderAsync(int videoId, int displayOrder);
    }
}