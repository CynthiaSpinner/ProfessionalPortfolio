using Microsoft.AspNetCore.Http;

namespace Portfolio.Services.Interfaces
{
    public interface IFileService
    {
        // General file operations
        Task<string> UploadFileAsync(IFormFile file, string subdirectory);
        Task<bool> DeleteFileAsync(string filePath);
        Task<string> GetFileUrlAsync(string filePath);

        // Video-specific operations
        Task<string> UploadVideoAsync(IFormFile videoFile, string subdirectory);
        Task<Stream> GetVideoStreamAsync(string filePath);
        Task<string> GetVideoThumbnailAsync(string videoPath);
        Task<bool> DeleteVideoAsync(string videoPath);

        // File validation
        Task<bool> ValidateFileSizeAsync(IFormFile file, long maxSize);
        Task<bool> ValidateFileTypeAsync(IFormFile file, string[] allowedTypes);
    }
}