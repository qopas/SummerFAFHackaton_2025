using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Ediki.Application.Interfaces;
using Ediki.Domain.Enums;
using System.Collections.Concurrent;

namespace Ediki.Api.Hubs;

[Authorize]
public class DemoHub : Hub
{
    private readonly ICurrentUserService _currentUserService;

    // Thread-safe dictionaries to track connections and sessions
    private static readonly ConcurrentDictionary<string, string> UserConnections = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> SessionParticipants = new();

    public DemoHub(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task JoinDemoSession(string sessionId, string participantId, string displayName)
    {
        try
        {
            // Add connection to session group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"demo-{sessionId}");
            
            // Track user connection
            UserConnections[Context.ConnectionId] = participantId;
            
            // Track session participants
            if (!SessionParticipants.ContainsKey(sessionId))
            {
                SessionParticipants[sessionId] = new HashSet<string>();
            }
            SessionParticipants[sessionId].Add(participantId);

            // Notify other participants
            await Clients.GroupExcept($"demo-{sessionId}", Context.ConnectionId)
                .SendAsync("ParticipantJoined", new
                {
                    SessionId = sessionId,
                    ParticipantId = participantId,
                    DisplayName = displayName,
                    ConnectionId = Context.ConnectionId,
                    JoinedAt = DateTime.UtcNow
                });

            // Send current participants list to the new participant
            var participants = SessionParticipants.GetValueOrDefault(sessionId, new HashSet<string>());
            await Clients.Caller.SendAsync("CurrentParticipants", new
            {
                SessionId = sessionId,
                Participants = participants.ToArray(),
                Count = participants.Count
            });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to join session",
                Details = ex.Message
            });
        }
    }

    public async Task LeaveDemoSession(string sessionId)
    {
        try
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"demo-{sessionId}");
            
            if (UserConnections.TryRemove(Context.ConnectionId, out var participantId))
            {
                // Remove from session participants
                if (SessionParticipants.ContainsKey(sessionId))
                {
                    SessionParticipants[sessionId].Remove(participantId);
                    if (!SessionParticipants[sessionId].Any())
                    {
                        SessionParticipants.TryRemove(sessionId, out _);
                    }
                }

                // Notify other participants
                await Clients.Group($"demo-{sessionId}")
                    .SendAsync("ParticipantLeft", new
                    {
                        SessionId = sessionId,
                        ParticipantId = participantId,
                        LeftAt = DateTime.UtcNow
                    });
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to leave session",
                Details = ex.Message
            });
        }
    }

    // WebRTC Signaling Methods
    public async Task SendWebRTCSignal(string sessionId, string targetParticipantId, string type, object data)
    {
        try
        {
            var fromParticipantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            if (!string.IsNullOrEmpty(targetParticipantId))
            {
                // Send to specific participant
                await Clients.Group($"demo-{sessionId}")
                    .SendAsync("ReceiveWebRTCSignal", new
                    {
                        SessionId = sessionId,
                        FromParticipantId = fromParticipantId,
                        TargetParticipantId = targetParticipantId,
                        Type = type,
                        Data = data,
                        Timestamp = DateTime.UtcNow
                    });
            }
            else
            {
                // Broadcast to all participants except sender
                await Clients.GroupExcept($"demo-{sessionId}", Context.ConnectionId)
                    .SendAsync("ReceiveWebRTCSignal", new
                    {
                        SessionId = sessionId,
                        FromParticipantId = fromParticipantId,
                        Type = type,
                        Data = data,
                        Timestamp = DateTime.UtcNow
                    });
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to send WebRTC signal",
                Details = ex.Message
            });
        }
    }

    // Screen sharing
    public async Task StartScreenShare(string sessionId)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.GroupExcept($"demo-{sessionId}", Context.ConnectionId)
                .SendAsync("ScreenShareStarted", new
                {
                    SessionId = sessionId,
                    ParticipantId = participantId,
                    StartedAt = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to start screen share",
                Details = ex.Message
            });
        }
    }

    public async Task StopScreenShare(string sessionId)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.Group($"demo-{sessionId}")
                .SendAsync("ScreenShareStopped", new
                {
                    SessionId = sessionId,
                    ParticipantId = participantId,
                    StoppedAt = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to stop screen share",
                Details = ex.Message
            });
        }
    }

    // Recording controls
    public async Task StartRecording(string sessionId)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.Group($"demo-{sessionId}")
                .SendAsync("RecordingStarted", new
                {
                    SessionId = sessionId,
                    StartedBy = participantId,
                    StartedAt = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to start recording",
                Details = ex.Message
            });
        }
    }

    public async Task StopRecording(string sessionId)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.Group($"demo-{sessionId}")
                .SendAsync("RecordingStopped", new
                {
                    SessionId = sessionId,
                    StoppedBy = participantId,
                    StoppedAt = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to stop recording",
                Details = ex.Message
            });
        }
    }

    // Chat functionality
    public async Task SendChatMessage(string sessionId, string message)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.Group($"demo-{sessionId}")
                .SendAsync("ReceiveChatMessage", new
                {
                    SessionId = sessionId,
                    ParticipantId = participantId,
                    Message = message,
                    Timestamp = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to send chat message",
                Details = ex.Message
            });
        }
    }

    // Media controls
    public async Task ToggleAudio(string sessionId, bool enabled)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.GroupExcept($"demo-{sessionId}", Context.ConnectionId)
                .SendAsync("ParticipantAudioToggled", new
                {
                    SessionId = sessionId,
                    ParticipantId = participantId,
                    AudioEnabled = enabled,
                    Timestamp = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to toggle audio",
                Details = ex.Message
            });
        }
    }

    public async Task ToggleVideo(string sessionId, bool enabled)
    {
        try
        {
            var participantId = UserConnections.GetValueOrDefault(Context.ConnectionId, "unknown");
            
            await Clients.GroupExcept($"demo-{sessionId}", Context.ConnectionId)
                .SendAsync("ParticipantVideoToggled", new
                {
                    SessionId = sessionId,
                    ParticipantId = participantId,
                    VideoEnabled = enabled,
                    Timestamp = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to toggle video",
                Details = ex.Message
            });
        }
    }

    // Session lifecycle
    public async Task StartSession(string sessionId)
    {
        try
        {
            await Clients.Group($"demo-{sessionId}")
                .SendAsync("SessionStarted", new
                {
                    SessionId = sessionId,
                    StartedAt = DateTime.UtcNow
                });
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to start session",
                Details = ex.Message
            });
        }
    }

    public async Task EndSession(string sessionId)
    {
        try
        {
            await Clients.Group($"demo-{sessionId}")
                .SendAsync("SessionEnded", new
                {
                    SessionId = sessionId,
                    EndedAt = DateTime.UtcNow
                });

            // Clean up session participants
            SessionParticipants.TryRemove(sessionId, out _);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", new
            {
                Message = "Failed to end session",
                Details = ex.Message
            });
        }
    }

    // Connection lifecycle
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            if (UserConnections.TryRemove(Context.ConnectionId, out var participantId))
            {
                // Find and clean up from all sessions
                var sessionsToCleanup = SessionParticipants
                    .Where(kvp => kvp.Value.Contains(participantId))
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var sessionId in sessionsToCleanup)
                {
                    SessionParticipants[sessionId].Remove(participantId);
                    if (!SessionParticipants[sessionId].Any())
                    {
                        SessionParticipants.TryRemove(sessionId, out _);
                    }

                    // Notify other participants
                    await Clients.Group($"demo-{sessionId}")
                        .SendAsync("ParticipantDisconnected", new
                        {
                            SessionId = sessionId,
                            ParticipantId = participantId,
                            DisconnectedAt = DateTime.UtcNow,
                            Reason = exception?.Message ?? "Connection lost"
                        });
                }
            }
        }
        catch (Exception ex)
        {
            // Log error but don't throw
            Console.WriteLine($"Error in OnDisconnectedAsync: {ex.Message}");
        }

        await base.OnDisconnectedAsync(exception);
    }

    // Health check
    public async Task Ping(string sessionId)
    {
        await Clients.Caller.SendAsync("Pong", new
        {
            SessionId = sessionId,
            Timestamp = DateTime.UtcNow,
            ConnectionId = Context.ConnectionId
        });
    }
} 