using Ediki.Application.Common.Interfaces;
using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Application.Interfaces;
using Ediki.Domain.Common;
using Ediki.Domain.Entities;
using Ediki.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Ediki.Domain.Enums;

namespace Ediki.Application.Features.ProjectMembers.Commands.InviteUserToProject;

public class InviteUserToProjectCommandHandler : IRequestHandler<InviteUserToProjectCommand, Result<ProjectMemberDto>>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationRepository _notificationRepository;

    public InviteUserToProjectCommandHandler(
        IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,
        UserManager<ApplicationUser> userManager,
        ICurrentUserService currentUserService,
        INotificationRepository notificationRepository)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<ProjectMemberDto>> Handle(InviteUserToProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

            // Check if project exists
            var project = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (project == null)
            {
                return Result<ProjectMemberDto>.Failure("Project not found");
            }

            // Check if current user is project lead or creator
            var currentUserMember = await _projectMemberRepository.GetByProjectAndUserAsync(request.ProjectId, currentUserId);
            bool isProjectCreator = project.CreatedById == currentUserId;
            bool isProjectLead = currentUserMember?.IsProjectLead == true;

            if (!isProjectCreator && !isProjectLead)
            {
                return Result<ProjectMemberDto>.Failure("Only project creators and project leads can invite users");
            }

            // Check if user exists
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null || user.IsDeleted)
            {
                return Result<ProjectMemberDto>.Failure("User not found");
            }

            // Check if user is already a member or has pending invitation
            var existingMember = await _projectMemberRepository.GetByProjectAndUserAsync(request.ProjectId, request.UserId);
            if (existingMember != null)
            {
                if (existingMember.IsActive)
                {
                    return Result<ProjectMemberDto>.Failure("User is already a member of this project");
                }
                if (existingMember.InvitedAt != null && existingMember.AcceptedAt == null)
                {
                    return Result<ProjectMemberDto>.Failure("User already has a pending invitation to this project");
                }
            }

            // Check if project needs this role
            if (!project.RolesNeeded.Contains(request.Role))
            {
                return Result<ProjectMemberDto>.Failure($"Project doesn't need {request.Role} role");
            }

            // Check if project has available slots
            var currentMemberCount = await _projectMemberRepository.GetActiveMemberCountAsync(request.ProjectId);
            if (currentMemberCount >= project.MaxParticipants)
            {
                return Result<ProjectMemberDto>.Failure("Project has reached maximum capacity");
            }

            // Create invitation
            var projectMember = ProjectMember.CreateInvitation(
                request.ProjectId,
                request.UserId,
                request.Role,
                currentUserId);

            await _projectMemberRepository.AddAsync(projectMember);

            // Get inviter information
            var inviter = await _userManager.FindByIdAsync(currentUserId);
            var inviterName = inviter != null ? $"{inviter.FirstName} {inviter.LastName}".Trim() : "Unknown";

            // Create notification
            var notification = new Notification
            {
                UserId = request.UserId,
                Type = NotificationType.ProjectInvitation,
                Priority = NotificationPriority.High,
                Title = "Project Invitation",
                Message = $"{inviterName} invited you to join the project '{project.Title}' as {request.Role}",
                ActionUrl = $"/projects/{project.Id}",
                RelatedEntityId = project.Id,
                RelatedEntityType = "Project",
                CreatedByUserId = currentUserId
            };

            await _notificationRepository.CreateAsync(notification);

            var projectMemberDto = new ProjectMemberDto
            {
                Id = projectMember.Id,
                ProjectId = projectMember.ProjectId,
                UserId = projectMember.UserId,
                UserName = user.UserName ?? string.Empty,
                UserEmail = user.Email ?? string.Empty,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserPreferredRole = user.PreferredRole,
                Role = projectMember.Role,
                JoinedAt = projectMember.JoinedAt,
                Progress = projectMember.Progress,
                IsActive = projectMember.IsActive,
                InvitedBy = projectMember.InvitedBy,
                InvitedByName = inviterName,
                InvitedAt = projectMember.InvitedAt,
                AcceptedAt = projectMember.AcceptedAt,
                IsProjectLead = projectMember.IsProjectLead
            };

            return Result<ProjectMemberDto>.Success(projectMemberDto);
        }
        catch (Exception ex)
        {
            return Result<ProjectMemberDto>.Failure($"Error inviting user to project: {ex.Message}");
        }
    }
} 