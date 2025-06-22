using System.Collections.Concurrent;
using System.Text.Json;

namespace Ediki.Api.Services;

public interface IMediaServerService
{
    Task<MediaServerStatus> GetStatusAsync();
    Task<string> CreateRoomAsync(string sessionId, MediaRoomConfig config);
    Task<bool> DestroyRoomAsync(string roomId);
    Task<RecordingInfo> StartRecordingAsync(string roomId, RecordingConfig config);
    Task<bool> StopRecordingAsync(string recordingId);
    Task<string> SaveRecordingAsync(string recordingId, Stream fileStream, string fileName);
    Task<IEnumerable<RecordingInfo>> GetRecordingsAsync(string sessionId);
}

public class MediaServerService : IMediaServerService
{
    private readonly ILogger<MediaServerService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _recordingsPath;
    
    // In-memory tracking for demo purposes
    private static readonly ConcurrentDictionary<string, MediaRoom> ActiveRooms = new();
    private static readonly ConcurrentDictionary<string, RecordingInfo> ActiveRecordings = new();
    
    public MediaServerService(ILogger<MediaServerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _recordingsPath = configuration.GetValue<string>("MediaServer:RecordingsPath") ?? "recordings";
        
        // Ensure recordings directory exists
        Directory.CreateDirectory(_recordingsPath);
    }

    public async Task<MediaServerStatus> GetStatusAsync()
    {
        await Task.Delay(1); // Simulate async operation
        
        return new MediaServerStatus
        {
            Status = "online",
            Version = "1.0.0",
            ConnectedSessions = ActiveRooms.Count,
            ActiveRecordings = ActiveRecordings.Count(r => r.Value.Status == RecordingStatus.Recording),
            ServerLoad = Random.Shared.Next(10, 50),
            Uptime = TimeSpan.FromHours(Random.Shared.Next(1, 48)),
            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task<string> CreateRoomAsync(string sessionId, MediaRoomConfig config)
    {
        await Task.Delay(1); // Simulate async operation
        
        var roomId = $"room-{sessionId}";
        var room = new MediaRoom
        {
            Id = roomId,
            SessionId = sessionId,
            Config = config,
            CreatedAt = DateTime.UtcNow,
            Participants = new List<string>(),
            Status = MediaRoomStatus.Active
        };
        
        ActiveRooms[roomId] = room;
        
        _logger.LogInformation("Created media room {RoomId} for session {SessionId}", roomId, sessionId);
        
        return roomId;
    }

    public async Task<bool> DestroyRoomAsync(string roomId)
    {
        await Task.Delay(1); // Simulate async operation
        
        if (ActiveRooms.TryRemove(roomId, out var room))
        {
            // Stop any active recordings in this room
            var roomRecordings = ActiveRecordings.Values
                .Where(r => r.RoomId == roomId && r.Status == RecordingStatus.Recording)
                .ToList();
                
            foreach (var recording in roomRecordings)
            {
                await StopRecordingAsync(recording.Id);
            }
            
            _logger.LogInformation("Destroyed media room {RoomId}", roomId);
            return true;
        }
        
        return false;
    }

    public async Task<RecordingInfo> StartRecordingAsync(string roomId, RecordingConfig config)
    {
        await Task.Delay(1); // Simulate async operation
        
        if (!ActiveRooms.ContainsKey(roomId))
        {
            throw new InvalidOperationException($"Room {roomId} not found");
        }
        
        var recordingId = Guid.NewGuid().ToString();
        var fileName = $"recording-{recordingId}.webm";
        var filePath = Path.Combine(_recordingsPath, fileName);
        
        var recording = new RecordingInfo
        {
            Id = recordingId,
            RoomId = roomId,
            Config = config,
            Status = RecordingStatus.Recording,
            StartedAt = DateTime.UtcNow,
            FileName = fileName,
            FilePath = filePath
        };
        
        // Create a dummy recording file
        try
        {
            Directory.CreateDirectory(_recordingsPath);
            await File.WriteAllTextAsync(filePath, $"Recording started at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\nRoom: {roomId}\nRecording ID: {recordingId}\nConfig: {System.Text.Json.JsonSerializer.Serialize(config)}");
            recording.FileSize = new FileInfo(filePath).Length;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create recording file {FilePath}", filePath);
        }
        
        ActiveRecordings[recordingId] = recording;
        
        _logger.LogInformation("Started recording {RecordingId} for room {RoomId}", recordingId, roomId);
        
        return recording;
    }

    public async Task<bool> StopRecordingAsync(string recordingId)
    {
        await Task.Delay(1); // Simulate async operation
        
        if (ActiveRecordings.TryGetValue(recordingId, out var recording))
        {
            recording.Status = RecordingStatus.Processing;
            recording.CompletedAt = DateTime.UtcNow;
            recording.Duration = recording.CompletedAt.Value - recording.StartedAt;
            
            // Update the recording file with stop information
            try
            {
                if (File.Exists(recording.FilePath))
                {
                    await File.AppendAllTextAsync(recording.FilePath, 
                        $"\nRecording stopped at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\nDuration: {recording.Duration:hh\\:mm\\:ss}");
                    recording.FileSize = new FileInfo(recording.FilePath).Length;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update recording file {FilePath}", recording.FilePath);
            }
            
            // Simulate processing delay
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5)); // Simulate processing
                recording.Status = RecordingStatus.Completed;
                recording.DownloadUrl = $"/api/v1/recordings/{recordingId}/download";
                
                // Final update to file
                try
                {
                    if (File.Exists(recording.FilePath))
                    {
                        await File.AppendAllTextAsync(recording.FilePath, 
                            $"\nProcessing completed at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}\nStatus: Completed");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to finalize recording file {FilePath}", recording.FilePath);
                }
            });
            
            _logger.LogInformation("Stopped recording {RecordingId}", recordingId);
            return true;
        }
        
        return false;
    }

    public async Task<string> SaveRecordingAsync(string recordingId, Stream fileStream, string fileName)
    {
        var filePath = Path.Combine(_recordingsPath, $"{recordingId}_{fileName}");
        
        using (var fileStreamWriter = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileStreamWriter);
        }
        
        // Update recording info if it exists
        if (ActiveRecordings.TryGetValue(recordingId, out var recording))
        {
            recording.FilePath = filePath;
            recording.FileName = fileName;
            recording.FileSize = new FileInfo(filePath).Length;
            recording.Status = RecordingStatus.Completed;
            recording.DownloadUrl = $"/api/v1/recordings/{recordingId}/download";
        }
        
        _logger.LogInformation("Saved recording file {FileName} for recording {RecordingId}", fileName, recordingId);
        
        return filePath;
    }

    public async Task<IEnumerable<RecordingInfo>> GetRecordingsAsync(string sessionId)
    {
        await Task.Delay(1); // Simulate async operation
        
        var roomId = $"room-{sessionId}";
        return ActiveRecordings.Values
            .Where(r => r.RoomId == roomId)
            .OrderByDescending(r => r.StartedAt)
            .ToList();
    }
}

// Data models
public class MediaServerStatus
{
    public string Status { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public int ConnectedSessions { get; set; }
    public int ActiveRecordings { get; set; }
    public double ServerLoad { get; set; }
    public TimeSpan Uptime { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class MediaRoom
{
    public string Id { get; set; } = string.Empty;
    public string SessionId { get; set; } = string.Empty;
    public MediaRoomConfig Config { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public List<string> Participants { get; set; } = new();
    public MediaRoomStatus Status { get; set; }
}

public class MediaRoomConfig
{
    public bool EnableRecording { get; set; } = false;
    public bool EnableScreenShare { get; set; } = true;
    public int MaxParticipants { get; set; } = 50;
    public string RecordingQuality { get; set; } = "HD";
    public List<string> IceServers { get; set; } = new()
    {
        "stun:stun.l.google.com:19302",
        "stun:stun1.l.google.com:19302"
    };
}

public class RecordingInfo
{
    public string Id { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public RecordingConfig Config { get; set; } = new();
    public RecordingStatus Status { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TimeSpan Duration { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string? DownloadUrl { get; set; }
}

public class RecordingConfig
{
    public string Quality { get; set; } = "HD";
    public string Format { get; set; } = "webm";
    public bool IncludeAudio { get; set; } = true;
    public bool IncludeVideo { get; set; } = true;
    public bool IncludeScreenShare { get; set; } = true;
}

public enum MediaRoomStatus
{
    Active,
    Inactive,
    Destroyed
}

public enum RecordingStatus
{
    Recording,
    Processing,
    Completed,
    Failed
} 