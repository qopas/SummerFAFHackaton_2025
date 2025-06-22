using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Ediki.Api.Hubs;
using Ediki.Api.Services;
using Ediki.Domain.Enums;
using SummerFAFHackaton_2025.Controllers;
using MediatR;
using Ediki.Application.Features.Demo.Commands.CreateDemoSession;

namespace Ediki.Api.Controllers.V1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class DemoController(IMediator mediator, IHubContext<DemoHub> hubContext, IMediaServerService mediaServerService) : BaseApiController(mediator)
{
    private readonly IHubContext<DemoHub> _hubContext = hubContext;
    private readonly IMediaServerService _mediaServerService = mediaServerService;

    [HttpPost("sessions")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDemoSession([FromBody] CreateDemoSessionRequest request)
    {
        var command = new CreateDemoSessionCommand
        {
            Title = request.Title,
            Description = request.Description,
            Type = request.Type,
            ScheduledAt = request.ScheduledAt,
            MaxParticipants = request.MaxParticipants,
            AutoRecord = request.AutoRecord,
            RecordingQuality = request.RecordingQuality,
            AllowScreenShare = request.AllowScreenShare,
            Duration = request.Duration,
            IsPublic = request.IsPublic,
            RequireModeration = request.RequireModeration
        };

        Console.WriteLine($"🎯 Creating demo session: {request.Title}");
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"❌ Failed to create demo session: {result.Error}");
            return BadRequest(new
            {
                success = false,
                message = result.Error,
                errors = new[] { result.Error },
                timestamp = DateTime.UtcNow
            });
        }

        Console.WriteLine($"✅ Demo session created successfully: {result.Value.Id}");

        // Create media room for the session
        try
        {
            Console.WriteLine($"🏠 Attempting to create media room for session: {result.Value.Id}");
            
            var roomConfig = new MediaRoomConfig
            {
                EnableRecording = command.AutoRecord,
                EnableScreenShare = command.AllowScreenShare ?? true,
                MaxParticipants = command.MaxParticipants,
                RecordingQuality = command.RecordingQuality.ToString()
            };

            var roomId = await _mediaServerService.CreateRoomAsync(result.Value.Id, roomConfig);
            Console.WriteLine($"✅ Successfully created media room: {roomId} for session: {result.Value.Id}");
        }
        catch (Exception ex)
        {
            // Log error but don't fail the session creation
            Console.WriteLine($"❌ Failed to create media room for session {result.Value.Id}: {ex.Message}");
            Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
        }

        return CreatedAtAction(nameof(GetDemoSession), new { id = result.Value.Id }, new
        {
            success = true,
            data = result.Value,
            message = "Demo session created successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("sessions/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDemoSession(string id)
    {
        // TODO: Implement GetDemoSessionQuery
        return Ok(new {
            success = true,
            data = new {
                id = id,
                title = "Sample Demo",
                status = DemoSessionStatus.Scheduled
            },
            message = "Demo session retrieved successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("sessions/{id}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartDemoSession(string id)
    {
        // TODO: Implement StartDemoSessionCommand
        
        // Notify all participants via SignalR
        await _hubContext.Clients.Group($"demo-{id}").SendAsync("DemoSessionStarted", new
        {
            SessionId = id,
            StartedAt = DateTime.UtcNow
        });
        
        return Ok(new {
            success = true,
            message = "Demo session started successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("sessions/{id}/end")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EndDemoSession(string id)
    {
        // TODO: Implement EndDemoSessionCommand
        
        // Notify all participants via SignalR
        await _hubContext.Clients.Group($"demo-{id}").SendAsync("DemoSessionEnded", new
        {
            SessionId = id,
            EndedAt = DateTime.UtcNow
        });
        
        return Ok(new {
            success = true,
            message = "Demo session ended successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("sessions/{id}/invite")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InviteParticipants(string id, [FromBody] InviteParticipantsRequest request)
    {
        // TODO: Implement InviteParticipantsCommand
        
        return Ok(new {
            success = true,
            data = new {
                invited = request.Emails.Count,
                inviteLink = $"https://yourdomain.com/demo/join/{id}"
            },
            message = "Participants invited successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("sessions/{id}/recording/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StartRecording(string id, [FromBody] StartRecordingRequest request)
    {
        try
        {
            var roomId = $"room-{id}";
            Console.WriteLine($"🎬 Attempting to start recording for session: {id}, roomId: {roomId}");
            
            var recordingConfig = new RecordingConfig
            {
                Quality = request.Quality.ToString(),
                Format = "webm",
                IncludeAudio = true,
                IncludeVideo = true,
                IncludeScreenShare = true
            };

            var recording = await _mediaServerService.StartRecordingAsync(roomId, recordingConfig);
            Console.WriteLine($"✅ Recording started successfully: {recording.Id}");
            
            // Notify participants via SignalR
            await _hubContext.Clients.Group($"demo-{id}").SendAsync("RecordingStarted", new
            {
                SessionId = id,
                RecordingId = recording.Id,
                Quality = request.Quality,
                StartedAt = DateTime.UtcNow
            });
            
            return Ok(new {
                success = true,
                data = new {
                    recordingId = recording.Id,
                    quality = request.Quality,
                    fileName = recording.FileName,
                    filePath = recording.FilePath,
                    status = recording.Status.ToString()
                },
                message = "Recording started successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to start recording for session {id}: {ex.Message}");
            Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
            
            return StatusCode(500, new {
                success = false,
                message = "Failed to start recording",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpPost("sessions/{id}/recording/stop")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StopRecording(string id)
    {
        try
        {
            Console.WriteLine($"🛑 Attempting to stop recording for session: {id}");
            
            // In a real implementation, you would get the active recording ID for this session
            // For now, we'll simulate stopping the most recent recording
            var recordings = await _mediaServerService.GetRecordingsAsync(id);
            var activeRecording = recordings.FirstOrDefault(r => r.Status == Ediki.Api.Services.RecordingStatus.Recording);
            
            if (activeRecording == null)
            {
                Console.WriteLine($"⚠️ No active recording found for session: {id}");
                return BadRequest(new {
                    success = false,
                    message = "No active recording found for this session",
                    errors = new[] { "No recording is currently active" },
                    timestamp = DateTime.UtcNow
                });
            }

            var stopped = await _mediaServerService.StopRecordingAsync(activeRecording.Id);
            
            if (!stopped)
            {
                Console.WriteLine($"❌ Failed to stop recording: {activeRecording.Id}");
                return StatusCode(500, new {
                    success = false,
                    message = "Failed to stop recording",
                    errors = new[] { "Recording could not be stopped" },
                    timestamp = DateTime.UtcNow
                });
            }
            
            Console.WriteLine($"✅ Recording stopped successfully: {activeRecording.Id}");
            
            // Notify participants via SignalR
            await _hubContext.Clients.Group($"demo-{id}").SendAsync("RecordingStopped", new
            {
                SessionId = id,
                RecordingId = activeRecording.Id,
                StoppedAt = DateTime.UtcNow,
                FilePath = activeRecording.FilePath,
                Duration = activeRecording.Duration.ToString(@"hh\:mm\:ss")
            });
            
            return Ok(new {
                success = true,
                data = new {
                    recordingId = activeRecording.Id,
                    fileName = activeRecording.FileName,
                    filePath = activeRecording.FilePath,
                    duration = activeRecording.Duration.ToString(@"hh\:mm\:ss"),
                    fileSize = activeRecording.FileSize,
                    downloadUrl = activeRecording.DownloadUrl
                },
                message = "Recording stopped successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to stop recording for session {id}: {ex.Message}");
            Console.WriteLine($"❌ Stack trace: {ex.StackTrace}");
            
            return StatusCode(500, new {
                success = false,
                message = "Failed to stop recording",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("sessions/{id}/recordings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecordings(string id)
    {
        try
        {
            Console.WriteLine($"📋 Getting recordings for session: {id}");
            
            // Get recordings from MediaServerService (in-memory)
            var recordings = await _mediaServerService.GetRecordingsAsync(id);
            var recordingsList = recordings.ToList();
            
            Console.WriteLine($"📊 Found {recordingsList.Count} recordings in memory for session: {id}");
            
            // Also check physical files in recordings directory
            var recordingsPath = Path.Combine(Directory.GetCurrentDirectory(), "recordings");
            var physicalFiles = new List<object>();
            
            if (Directory.Exists(recordingsPath))
            {
                var files = Directory.GetFiles(recordingsPath, "*.webm");
                Console.WriteLine($"📁 Found {files.Length} physical recording files");
                
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    var fileInfo = new FileInfo(file);
                    
                    // Try to extract recordingId from filename (format: recordingId_originalFileName.webm)
                    var parts = fileName.Split('_', 2);
                    var recordingId = parts.Length > 0 ? parts[0] : Guid.NewGuid().ToString();
                    
                    // Check if this recording is associated with the session
                    var associatedRecording = recordingsList.FirstOrDefault(r => r.Id == recordingId);
                    
                    if (associatedRecording != null)
                    {
                        // Use data from memory
                        physicalFiles.Add(new {
                            id = associatedRecording.Id,
                            fileName = associatedRecording.FileName,
                            fileSize = fileInfo.Length,
                            duration = associatedRecording.Duration.ToString(@"hh\:mm\:ss"),
                            quality = associatedRecording.Config.Quality,
                            status = associatedRecording.Status.ToString(),
                            startedAt = associatedRecording.StartedAt,
                            completedAt = associatedRecording.CompletedAt,
                            downloadUrl = $"/api/v1/recordings/{associatedRecording.Id}/download",
                            filePath = file
                        });
                    }
                    else
                    {
                        // Orphaned file - create basic info
                        physicalFiles.Add(new {
                            id = recordingId,
                            fileName = fileName,
                            fileSize = fileInfo.Length,
                            duration = "Unknown",
                            quality = "Unknown",
                            status = "Completed",
                            startedAt = fileInfo.CreationTime,
                            completedAt = (DateTime?)fileInfo.LastWriteTime,
                            downloadUrl = $"/api/v1/recordings/{recordingId}/download",
                            filePath = file
                        });
                    }
                }
            }
            
            Console.WriteLine($"✅ Returning {physicalFiles.Count} recordings for session: {id}");
            
            return Ok(new {
                success = true,
                data = physicalFiles,
                message = $"Found {physicalFiles.Count} recordings for session {id}",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error getting recordings for session {id}: {ex.Message}");
            return StatusCode(500, new {
                success = false,
                message = "Error retrieving recordings",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    [HttpGet("sessions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDemoSessions([FromQuery] DemoSessionStatus? status = null)
    {
        // TODO: Implement GetDemoSessionsQuery
        
        return Ok(new {
            success = true,
            data = new[] {
                new {
                    id = Guid.NewGuid().ToString(),
                    title = "Product Demo",
                    status = DemoSessionStatus.Scheduled,
                    scheduledAt = DateTime.UtcNow.AddHours(1),
                    participantCount = 5
                }
            },
            message = "Demo sessions retrieved successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    // Participants Management
    [HttpGet("sessions/{id}/participants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetParticipants(string id)
    {
        // TODO: Implement GetParticipantsQuery
        
        return Ok(new {
            success = true,
            data = new[] {
                new {
                    id = Guid.NewGuid().ToString(),
                    userId = Guid.NewGuid().ToString(),
                    displayName = "John Doe",
                    email = "john@example.com",
                    role = DemoParticipantRole.Participant,
                    status = DemoParticipantStatus.Joined,
                    hasVideo = true,
                    hasAudio = true,
                    canInteract = true,
                    joinedAt = DateTime.UtcNow.AddMinutes(-15)
                }
            },
            message = "Participants retrieved successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("sessions/{id}/join")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> JoinSession(string id, [FromBody] JoinSessionRequest request)
    {
        // TODO: Implement JoinSessionCommand
        
        var participantId = Guid.NewGuid().ToString();
        
        // Notify all participants via SignalR
        await _hubContext.Clients.Group($"demo-{id}").SendAsync("ParticipantJoined", new
        {
            SessionId = id,
            ParticipantId = participantId,
            DisplayName = request.DisplayName,
            JoinedAt = DateTime.UtcNow
        });
        
        return Ok(new {
            success = true,
            data = new {
                participantId = participantId,
                sessionId = id,
                roomId = $"room-{id}",
                iceServers = new[] {
                    new { urls = "stun:stun.l.google.com:19302" },
                    new { urls = "stun:stun1.l.google.com:19302" }
                }
            },
            message = "Successfully joined session",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpPost("sessions/{id}/leave")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LeaveSession(string id)
    {
        // TODO: Implement LeaveSessionCommand
        
        // Notify all participants via SignalR
        await _hubContext.Clients.Group($"demo-{id}").SendAsync("ParticipantLeft", new
        {
            SessionId = id,
            LeftAt = DateTime.UtcNow
        });
        
        return Ok(new {
            success = true,
            message = "Successfully left session",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    // WebRTC Signaling
    [HttpPost("sessions/{id}/signal")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SendSignal(string id, [FromBody] WebRTCSignalRequest request)
    {
        // Forward signal via SignalR to specific participant or broadcast
        if (!string.IsNullOrEmpty(request.TargetParticipantId))
        {
            // Send to specific participant
            await _hubContext.Clients.Group($"demo-{id}")
                .SendAsync("ReceiveSignal", new
                {
                    SessionId = id,
                    FromParticipantId = request.FromParticipantId,
                    TargetParticipantId = request.TargetParticipantId,
                    Type = request.Type,
                    Data = request.Data,
                    Timestamp = DateTime.UtcNow
                });
        }
        else
        {
            // Broadcast to all participants
            await _hubContext.Clients.Group($"demo-{id}")
                .SendAsync("BroadcastSignal", new
                {
                    SessionId = id,
                    FromParticipantId = request.FromParticipantId,
                    Type = request.Type,
                    Data = request.Data,
                    Timestamp = DateTime.UtcNow
                });
        }
        
        return Ok(new {
            success = true,
            message = "Signal sent successfully",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("sessions/{id}/signals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSignals(string id, [FromQuery] DateTime? since = null)
    {
        // TODO: Implement GetSignalsQuery - for now return empty as signals are real-time via SignalR
        
        return Ok(new {
            success = true,
            data = new object[0], // Signals are handled via SignalR in real-time
            message = "Signals retrieved successfully (use SignalR for real-time)",
            errors = new string[0],
            timestamp = DateTime.UtcNow
        });
    }

    // Recording Upload
    [HttpPost("sessions/{id}/recordings/upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
    public async Task<IActionResult> UploadRecording(string id, [FromForm] UploadRecordingRequest request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest(new {
                success = false,
                message = "No file provided",
                errors = new[] { "File is required" },
                timestamp = DateTime.UtcNow
            });
        }

        // Check file size (max 500MB for demo)
        const long maxFileSize = 500 * 1024 * 1024; // 500MB
        if (request.File.Length > maxFileSize)
        {
            return StatusCode(413, new {
                success = false,
                message = "File too large",
                errors = new[] { "Maximum file size is 500MB" },
                timestamp = DateTime.UtcNow
            });
        }

        try
        {
            var recordingId = Guid.NewGuid().ToString();
            var filePath = await _mediaServerService.SaveRecordingAsync(recordingId, request.File.OpenReadStream(), request.File.FileName);
            
            return Ok(new {
                success = true,
                data = new {
                    recordingId = recordingId,
                    fileName = request.File.FileName,
                    fileSize = request.File.Length,
                    filePath = filePath,
                    uploadedAt = DateTime.UtcNow,
                    status = Ediki.Domain.Enums.RecordingStatus.Completed,
                    downloadUrl = $"/api/v1/recordings/{recordingId}/download"
                },
                message = "Recording uploaded successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Failed to upload recording",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    // Streaming upload for large recordings
    [HttpPost("sessions/{id}/recordings/upload-stream")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadRecordingStream(string id, [FromQuery] string fileName, [FromQuery] string? recordingId = null)
    {
        try
        {
            recordingId ??= Guid.NewGuid().ToString();
            
            // Read the stream directly from request body
            using var stream = Request.Body;
            var filePath = await _mediaServerService.SaveRecordingAsync(recordingId, stream, fileName);
            
            return Ok(new {
                success = true,
                data = new {
                    recordingId = recordingId,
                    fileName = fileName,
                    filePath = filePath,
                    uploadedAt = DateTime.UtcNow,
                    status = Ediki.Domain.Enums.RecordingStatus.Completed,
                    downloadUrl = $"/api/v1/recordings/{recordingId}/download"
                },
                message = "Recording stream uploaded successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Failed to upload recording stream",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    // Media Server Status
    [HttpGet("media-server/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMediaServerStatus()
    {
        try
        {
            var status = await _mediaServerService.GetStatusAsync();
            
            return Ok(new {
                success = true,
                data = new {
                    status = status.Status,
                    version = status.Version,
                    connectedSessions = status.ConnectedSessions,
                    activeRecordings = status.ActiveRecordings,
                    serverLoad = status.ServerLoad,
                    uptime = status.Uptime.ToString(@"dd\.hh\:mm\:ss"),
                    lastUpdated = status.LastUpdated
                },
                message = "Media server status retrieved",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Failed to get media server status",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    // Manual room creation for debugging
    [HttpPost("sessions/{id}/create-room")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMediaRoom(string id, [FromBody] CreateRoomRequest? request = null)
    {
        try
        {
            var roomConfig = new MediaRoomConfig
            {
                EnableRecording = request?.EnableRecording ?? true,
                EnableScreenShare = request?.EnableScreenShare ?? true,
                MaxParticipants = request?.MaxParticipants ?? 50,
                RecordingQuality = request?.RecordingQuality ?? "HD"
            };

            var roomId = await _mediaServerService.CreateRoomAsync(id, roomConfig);
            
            return Ok(new {
                success = true,
                data = new {
                    sessionId = id,
                    roomId = roomId,
                    config = roomConfig
                },
                message = "Media room created successfully",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Failed to create media room",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }

    // Check room status for debugging
    [HttpGet("sessions/{id}/room-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoomStatus(string id)
    {
        try
        {
            var roomId = $"room-{id}";
            var status = await _mediaServerService.GetStatusAsync();
            
            // Get all active rooms from MediaServerService
            var mediaServerType = _mediaServerService.GetType();
            var activeRoomsField = mediaServerType.GetField("ActiveRooms", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            
            var activeRooms = activeRoomsField?.GetValue(null);
            var roomExists = false;
            
            if (activeRooms != null)
            {
                var method = activeRooms.GetType().GetMethod("ContainsKey");
                roomExists = (bool)(method?.Invoke(activeRooms, new object[] { roomId }) ?? false);
            }
            
            return Ok(new {
                success = true,
                data = new {
                    sessionId = id,
                    roomId = roomId,
                    roomExists = roomExists,
                    totalActiveRooms = status.ConnectedSessions,
                    serverStatus = status.Status
                },
                message = "Room status retrieved",
                errors = new string[0],
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                success = false,
                message = "Failed to get room status",
                errors = new[] { ex.Message },
                timestamp = DateTime.UtcNow
            });
        }
    }
}

// Request DTOs
public class CreateDemoSessionRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DemoSessionType Type { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int MaxParticipants { get; set; } = 50;
    public bool AutoRecord { get; set; } = false;
    public RecordingQuality RecordingQuality { get; set; } = RecordingQuality.HD;
    
    // Additional optional fields from frontend
    public bool? AllowScreenShare { get; set; }
    public int? Duration { get; set; } // Duration in minutes
    public bool? IsPublic { get; set; }
    public bool? RequireModeration { get; set; }
}

public class InviteParticipantsRequest
{
    public List<string> Emails { get; set; } = new();
    public string? Message { get; set; }
}

public class StartRecordingRequest
{
    public RecordingQuality Quality { get; set; } = RecordingQuality.HD;
}

public class JoinSessionRequest
{
    public string DisplayName { get; set; } = string.Empty;
}

public class WebRTCSignalRequest
{
    public string FromParticipantId { get; set; } = string.Empty;
    public string TargetParticipantId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
}

public class UploadRecordingRequest
{
    public required IFormFile File { get; set; }
}

public class CreateRoomRequest
{
    public bool? EnableRecording { get; set; }
    public bool? EnableScreenShare { get; set; }
    public int? MaxParticipants { get; set; }
    public string? RecordingQuality { get; set; }
} 