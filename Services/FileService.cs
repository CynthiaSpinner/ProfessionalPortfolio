using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Portfolio.Services.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Portfolio.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;
        private readonly string[] _allowedVideoTypes = { ".mp4", ".webm", ".ogg" };
        private readonly long _maxVideoSize = 500 * 1024 * 1024; // 500MB

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Ensure upload directories exist
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            var videoPath = Path.Combine(_uploadPath, "videos");
            if (!Directory.Exists(videoPath))
            {
                Directory.CreateDirectory(videoPath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subdirectory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var directoryPath = Path.Combine(_uploadPath, subdirectory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", subdirectory, fileName).Replace("\\", "/");
        }

        public async Task<string> UploadVideoAsync(IFormFile videoFile, string subdirectory)
        {
            if (videoFile == null || videoFile.Length == 0)
                throw new ArgumentException("Video file is empty");

            // Validate video file
            if (!await ValidateFileTypeAsync(videoFile, _allowedVideoTypes))
                throw new ArgumentException("Invalid video file type");

            if (!await ValidateFileSizeAsync(videoFile, _maxVideoSize))
                throw new ArgumentException("Video file too large");

            var directoryPath = Path.Combine(_uploadPath, "videos", subdirectory);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(videoFile.FileName)}";
            var filePath = Path.Combine(directoryPath, fileName);

            // Use chunked upload for large files
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await videoFile.CopyToAsync(stream);
            }

            // Generate thumbnail
            await GenerateVideoThumbnailAsync(filePath);

            return Path.Combine("uploads", "videos", subdirectory, fileName).Replace("\\", "/");
        }

        public Task<Stream> GetVideoStreamAsync(string filePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Video file not found");

            return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true));
        }

        public async Task<string> GetVideoThumbnailAsync(string videoPath)
        {
            var thumbnailPath = videoPath.Replace(Path.GetExtension(videoPath), "_thumb.jpg");
            var fullThumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", thumbnailPath);

            if (!File.Exists(fullThumbnailPath))
            {
                await GenerateVideoThumbnailAsync(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", videoPath));
            }

            return thumbnailPath;
        }

        private Task GenerateVideoThumbnailAsync(string videoPath)
        {
            // This is a placeholder for actual video thumbnail generation
            // In a real implementation, you would use a library like FFmpeg
            // to extract a frame from the video and save it as a thumbnail
            var thumbnailPath = videoPath.Replace(Path.GetExtension(videoPath), "_thumb.jpg");

            // Create a placeholder thumbnail
            using (var image = new Bitmap(320, 180))
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Black);
                using (var font = new Font("Arial", 20))
                {
                    graphics.DrawString("Video Thumbnail", font, Brushes.White, new PointF(10, 10));
                }
                image.Save(thumbnailPath, ImageFormat.Jpeg);
            }
            
            return Task.CompletedTask;
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.FromResult(false);

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public Task<bool> DeleteVideoAsync(string videoPath)
        {
            if (string.IsNullOrEmpty(videoPath))
                return Task.FromResult(false);

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", videoPath);
            var thumbnailPath = videoPath.Replace(Path.GetExtension(videoPath), "_thumb.jpg");
            var fullThumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", thumbnailPath);

            bool deleted = false;
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                deleted = true;
            }

            if (File.Exists(fullThumbnailPath))
            {
                File.Delete(fullThumbnailPath);
            }

            return Task.FromResult(deleted);
        }

        public Task<string> GetFileUrlAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.FromResult(string.Empty);

            var baseUrl = _configuration["BaseUrl"] ?? "https://localhost:5001";
            return Task.FromResult($"{baseUrl}/{filePath}");
        }

        public Task<bool> ValidateFileSizeAsync(IFormFile file, long maxSize)
        {
            return Task.FromResult(file.Length <= maxSize);
        }

        public Task<bool> ValidateFileTypeAsync(IFormFile file, string[] allowedTypes)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return Task.FromResult(allowedTypes.Contains(fileExtension));
        }
    }
}