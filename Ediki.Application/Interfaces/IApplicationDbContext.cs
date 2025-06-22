using Ediki.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ediki.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<Team> Teams { get; set; }
    DbSet<TeamMember> TeamMembers { get; set; }
    DbSet<ProjectMember> ProjectMembers { get; set; }
    DbSet<Sprint> Sprints { get; set; }
    DbSet<Domain.Entities.Task> Tasks { get; set; }
    DbSet<Notification> Notifications { get; set; }
    DbSet<DemoSession> DemoSessions { get; set; }
    DbSet<DemoParticipant> DemoParticipants { get; set; }
    DbSet<DemoRecording> DemoRecordings { get; set; }
    DbSet<DemoMessage> DemoMessages { get; set; }
    DbSet<DemoAction> DemoActions { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 