using Ediki.Domain.Common;
using Ediki.Application.Interfaces;
using Ediki.Domain.Entities;
using Ediki.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Features.Demo.Commands.CreateDemoSession;

public class CreateDemoSessionCommandHandler : IRequestHandler<CreateDemoSessionCommand, Result<CreateDemoSessionResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateDemoSessionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CreateDemoSessionResponse>> Handle(CreateDemoSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        
        if (string.IsNullOrEmpty(userId))
        {
            return Result<CreateDemoSessionResponse>.Failure("User not authenticated");
        }

        // Check if user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        if (!userExists)
        {
            return Result<CreateDemoSessionResponse>.Failure("User not found");
        }

        var sessionId = Guid.NewGuid().ToString();
        var streamKey = Guid.NewGuid().ToString();
        var roomId = $"room-{sessionId}";

        var demoSession = new DemoSession
        {
            Id = sessionId,
            Title = request.Title,
            Description = request.Description,
            HostUserId = userId,
            Type = request.Type,
            Status = DemoSessionStatus.Scheduled,
            ScheduledAt = request.ScheduledAt,
            MaxParticipants = request.MaxParticipants,
            StreamKey = streamKey,
            RoomId = roomId,
            DurationMinutes = request.Duration,
            IsPublic = request.IsPublic ?? false,
            RequireModeration = request.RequireModeration ?? false,
            Settings = new DemoSettings
            {
                AllowChat = true,
                AllowQuestions = true,
                AllowScreenAnnotation = false,
                AllowScreenShare = request.AllowScreenShare ?? true,
                AutoRecord = request.AutoRecord,
                RequireApproval = request.RequireModeration ?? false,
                RecordingQuality = request.RecordingQuality,
                AllowedDomains = new List<string>()
            }
        };

        _context.DemoSessions.Add(demoSession);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new CreateDemoSessionResponse
        {
            Id = demoSession.Id,
            Title = demoSession.Title,
            ScheduledAt = demoSession.ScheduledAt,
            StreamKey = demoSession.StreamKey!,
            RoomId = demoSession.RoomId!,
            Status = demoSession.Status
        };

        return Result<CreateDemoSessionResponse>.Success(response);
    }
} 