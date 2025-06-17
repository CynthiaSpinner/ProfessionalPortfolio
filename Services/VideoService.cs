using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Services
{
    public class VideoService : IVideoService
    {
        private readonly PortfolioContext _context;
        private readonly IFileService _fileService;

        public VideoService(PortfolioContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Video> UploadVideoAsync(IFormFile videoFile, string title, string description)
        {
            // Upload video file
            var filePath = await _fileService.UploadVideoAsync(videoFile, "videos");
            var thumbnailPath = await _fileService.GetVideoThumbnailAsync(filePath);

            var video = new Video
            {
                Title = title,
                Description = description,
                FilePath = filePath,
                ThumbnailPath = thumbnailPath,
                FileType = Path.GetExtension(videoFile.FileName),
                FileSize = videoFile.Length,
                UploadDate = DateTime.UtcNow,
                IsActive = true,
                DisplayOrder = await GetNextDisplayOrderAsync()
            };

            _context.Videos.Add(video);
            await _context.SaveChangesAsync();

            return video;
        }

        public async Task<bool> DeleteVideoAsync(int videoId)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video == null)
                return false;

            // Delete video file and thumbnail
            await _fileService.DeleteVideoAsync(video.FilePath);

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Video> GetVideoByIdAsync(int videoId)
        {
            return await _context.Videos.FindAsync(videoId);
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            return await _context.Videos
                .OrderBy(v => v.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetActiveVideosAsync()
        {
            return await _context.Videos
                .Where(v => v.IsActive)
                .OrderBy(v => v.DisplayOrder)
                .ToListAsync();
        }

        public async Task<bool> UpdateVideoAsync(Video video)
        {
            var existingVideo = await _context.Videos.FindAsync(video.Id);
            if (existingVideo == null)
                return false;

            existingVideo.Title = video.Title;
            existingVideo.Description = video.Description;
            existingVideo.IsActive = video.IsActive;
            existingVideo.DisplayOrder = video.DisplayOrder;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateVideoStatusAsync(int videoId, bool isActive)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video == null)
                return false;

            video.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateVideoOrderAsync(int videoId, int displayOrder)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video == null)
                return false;

            video.DisplayOrder = displayOrder;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<int> GetNextDisplayOrderAsync()
        {
            var maxOrder = await _context.Videos.MaxAsync(v => (int?)v.DisplayOrder) ?? 0;
            return maxOrder + 1;
        }
    }
}