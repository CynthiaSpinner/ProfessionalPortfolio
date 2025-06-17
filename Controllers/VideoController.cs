using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.Portfolio;
using Portfolio.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Portfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<ActionResult<Video>> UploadVideo([FromForm] IFormFile videoFile, [FromForm] string title, [FromForm] string description)
        {
            if (videoFile == null || videoFile.Length == 0)
                return BadRequest("No video file provided");

            var video = await _videoService.UploadVideoAsync(videoFile, title, description);
            return Ok(video);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var result = await _videoService.DeleteVideoAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Video>> GetVideo(int id)
        {
            var video = await _videoService.GetVideoByIdAsync(id);
            if (video == null)
                return NotFound();

            return Ok(video);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetAllVideos()
        {
            var videos = await _videoService.GetAllVideosAsync();
            return Ok(videos);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Video>>> GetActiveVideos()
        {
            var videos = await _videoService.GetActiveVideosAsync();
            return Ok(videos);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVideo(int id, [FromBody] Video video)
        {
            if (id != video.Id)
                return BadRequest();

            var result = await _videoService.UpdateVideoAsync(video);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateVideoStatus(int id, [FromBody] bool isActive)
        {
            var result = await _videoService.UpdateVideoStatusAsync(id, isActive);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}/order")]
        [Authorize]
        public async Task<IActionResult> UpdateVideoOrder(int id, [FromBody] int displayOrder)
        {
            var result = await _videoService.UpdateVideoOrderAsync(id, displayOrder);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}