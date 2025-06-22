using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ediki.Api.Services;
using SummerFAFHackaton_2025.Controllers;
using MediatR;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class RecordingsController(IMediator mediator, IMediaServerService mediaServerService) : BaseApiController(mediator)
{
    private readonly IMediaServerService _mediaServerService = mediaServerService;

    [HttpGet("{recordingId}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadRecording(string recordingId)
    {
        try
        {
            Console.WriteLine($"📥 Download request for recording: {recordingId}");
            
            var recordingsPath = Path.Combine(Directory.GetCurrentDirectory(), "recordings");
            var files = Directory.GetFiles(recordingsPath, $"{recordingId}_*");
            
            if (!files.Any())
            {
                Console.WriteLine($"❌ Recording not found: {recordingId}");
                return NotFound(new {
                    success = false,
                    message = "Recording not found",
                    errors = new[] { "The requested recording does not exist" },
                    timestamp = DateTime.UtcNow
                });
            }
            
            var filePath = files.First();
            var fileName = Path.GetFileName(filePath);
            var fileInfo = new FileInfo(filePath);
            
            Console.WriteLine($"✅ Found recording file: {fileName} ({fileInfo.Length} bytes)");
            
            Response.Headers.Append("Access-Control-Allow-Origin", "*");
            Response.Headers.Append("Access-Control-Allow-Methods", "GET");
            Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
            
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.Append("Cache-Control", "public, max-age=3600");
            
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            
            return File(fileBytes, "video/webm", fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error downloading recording {recordingId}: {ex.Message}");
            return StatusCode(500, new {
                success = false,
                message = "Error downloading recording",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("session/{sessionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSessionRecordings(string sessionId)
    {
        try
        {
            var recordings = await _mediaServerService.GetRecordingsAsync(sessionId);
            
            return Ok(new {
                success = true,
                data = recordings.Select(r => new {
                    id = r.Id,
                    fileName = r.FileName,
                    duration = r.Duration.ToString(@"hh\:mm\:ss"),
                    fileSize = r.FileSize,
                    status = r.Status.ToString(),
                    startedAt = r.StartedAt,
                    completedAt = r.CompletedAt,
                    downloadUrl = r.DownloadUrl,
                    quality = r.Config.Quality
                }),
                message = "Session recordings retrieved successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error retrieving session recordings",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpDelete("{recordingId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRecording(string recordingId)
    {
        try
        {
            // In a real implementation, you would:
            // 1. Validate user has permission to delete this recording
            // 2. Remove from database
            // 3. Delete file from storage
            
            var recordingsPath = Path.Combine(Directory.GetCurrentDirectory(), "recordings");
            var files = Directory.GetFiles(recordingsPath, $"{recordingId}_*");
            
            if (!files.Any())
            {
                return NotFound(new {
                    success = false,
                    message = "Recording not found",
                    errors = new[] { "The requested recording does not exist" },
                    timestamp = DateTime.UtcNow
                });
            }
            
            foreach (var file in files)
            {
                System.IO.File.Delete(file);
            }
            
            return Ok(new {
                success = true,
                message = "Recording deleted successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error deleting recording",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRecordings([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            // In a real implementation, you would query the database with pagination
            var recordingsPath = Path.Combine(Directory.GetCurrentDirectory(), "recordings");
            
            if (!Directory.Exists(recordingsPath))
            {
                Directory.CreateDirectory(recordingsPath);
            }
            
            var files = Directory.GetFiles(recordingsPath, "*.webm")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new {
                    id = Path.GetFileNameWithoutExtension(f).Split('_').FirstOrDefault() ?? "unknown",
                    fileName = Path.GetFileName(f),
                    fileSize = new FileInfo(f).Length,
                    createdAt = System.IO.File.GetCreationTime(f),
                    downloadUrl = $"/api/v1/recordings/{Path.GetFileNameWithoutExtension(f).Split('_').FirstOrDefault()}/download"
                });
            
            return Ok(new {
                success = true,
                data = files,
                pagination = new {
                    page = page,
                    pageSize = pageSize,
                    totalCount = Directory.GetFiles(recordingsPath, "*.webm").Length
                },
                message = "Recordings retrieved successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Error retrieving recordings",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    // Streaming endpoint with Range support for better video playback
    [HttpGet("{recordingId}/stream")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status206PartialContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> StreamRecording(string recordingId)
    {
        try
        {
            Console.WriteLine($"🎬 Stream request for recording: {recordingId}");
            
            var recordingsPath = Path.Combine(Directory.GetCurrentDirectory(), "recordings");
            var files = Directory.GetFiles(recordingsPath, $"{recordingId}_*");
            
            if (!files.Any())
            {
                Console.WriteLine($"❌ Recording not found for streaming: {recordingId}");
                return NotFound();
            }
            
            var filePath = files.First();
            var fileInfo = new FileInfo(filePath);
            var fileLength = fileInfo.Length;
            
            Console.WriteLine($"🎥 Streaming file: {Path.GetFileName(filePath)} ({fileLength} bytes)");
            
            // Add CORS headers
            Response.Headers.Append("Access-Control-Allow-Origin", "*");
            Response.Headers.Append("Access-Control-Allow-Methods", "GET");
            Response.Headers.Append("Access-Control-Allow-Headers", "Range, Content-Type");
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.Append("Content-Length", fileLength.ToString());
            Response.Headers.Append("Content-Type", "video/webm");
            
            // Return file stream with range processing enabled
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "video/webm", enableRangeProcessing: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error streaming recording {recordingId}: {ex.Message}");
            return StatusCode(500);
        }
    }
} 