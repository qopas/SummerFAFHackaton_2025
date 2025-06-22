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
    public async Task<IActionResult> DownloadRecording(string recordingId)
    {
        try
        {
            // In a real implementation, you would:
            // 1. Validate user has access to this recording
            // 2. Get file path from database
            // 3. Return file stream
            
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
            
            var filePath = files.First();
            var fileName = Path.GetFileName(filePath);
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            
            return File(fileBytes, "video/webm", fileName);
        }
        catch (Exception ex)
        {
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
} 