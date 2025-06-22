using MediatR;
using Ediki.Domain.Common;
using Ediki.Application.Features.ProjectMembers.DTOs;
using Ediki.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Ediki.Domain.Entities;
using Ediki.Domain.Interfaces;
using Ediki.Domain.Enums;

namespace Ediki.Application.Features.ProjectMembers.Queries.GetProjectMembers;

public class GetProjectMembersQueryHandler(
    IProjectMemberRepository projectMemberRepository,
    IProjectRepository projectRepository,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetProjectMembersQuery, Result<IEnumerable<ProjectMemberDto>>>
{
    public async Task<Result<IEnumerable<ProjectMemberDto>>> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
    {
        // Получаем проект чтобы узнать создателя
        var project = await projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null)
        {
            return Result<IEnumerable<ProjectMemberDto>>.Failure("Project not found");
        }

        // Получаем участников проекта
        var members = await projectMemberRepository.GetProjectMembersAsync(
            request.ProjectId,
            request.IsActive ?? true);

        var filteredMembers = members.AsEnumerable();
        
        if (request.IsActive.HasValue)
        {
            filteredMembers = filteredMembers.Where(m => m.IsActive == request.IsActive.Value);
        }
        
        if (request.IsProjectLead.HasValue)
        {
            filteredMembers = filteredMembers.Where(m => m.IsProjectLead == request.IsProjectLead.Value);
        }

        var memberDtos = new List<ProjectMemberDto>();

        // Добавляем всех участников проекта
        memberDtos.AddRange(filteredMembers.Select(m => new ProjectMemberDto
        {
            Id = m.Id,
            ProjectId = m.ProjectId,
            UserId = m.UserId,
            UserName = m.User.UserName ?? string.Empty,
            UserEmail = m.User.Email ?? string.Empty,
            UserFirstName = m.User.FirstName,
            UserLastName = m.User.LastName,
            UserPreferredRole = m.User.PreferredRole,
            Role = m.Role,
            JoinedAt = m.JoinedAt,
            Progress = m.Progress,
            IsActive = m.IsActive,
            InvitedBy = m.InvitedBy,
            InvitedByName = m.InvitedByUser != null ? $"{m.InvitedByUser.FirstName} {m.InvitedByUser.LastName}".Trim() : null,
            InvitedAt = m.InvitedAt,
            AcceptedAt = m.AcceptedAt,
            IsProjectLead = m.IsProjectLead
        }));

        // Проверяем, есть ли создатель проекта уже в списке участников
        var creatorAlreadyMember = memberDtos.Any(m => m.UserId == project.CreatedById);
        
        if (!creatorAlreadyMember)
        {
            // Получаем информацию о создателе проекта
            var creator = await userManager.FindByIdAsync(project.CreatedById);
            if (creator != null)
            {
                // Добавляем создателя как участника с ролью лидера проекта
                var creatorDto = new ProjectMemberDto
                {
                    Id = $"creator-{project.Id}",
                    ProjectId = project.Id,
                    UserId = creator.Id,
                    UserName = creator.UserName ?? string.Empty,
                    UserEmail = creator.Email ?? string.Empty,
                    UserFirstName = creator.FirstName,
                    UserLastName = creator.LastName,
                    UserPreferredRole = creator.PreferredRole,
                    Role = ProjectRole.Developer, // Можно настроить роль по умолчанию для создателя
                    JoinedAt = project.CreatedAt,
                    Progress = 0.0f,
                    IsActive = true,
                    InvitedBy = null,
                    InvitedByName = null,
                    InvitedAt = null,
                    AcceptedAt = project.CreatedAt,
                    IsProjectLead = true
                };

                // Применяем фильтры к создателю
                bool includeCreator = true;
                
                if (request.IsActive.HasValue && creatorDto.IsActive != request.IsActive.Value)
                {
                    includeCreator = false;
                }
                
                if (request.IsProjectLead.HasValue && creatorDto.IsProjectLead != request.IsProjectLead.Value)
                {
                    includeCreator = false;
                }

                if (includeCreator)
                {
                    memberDtos.Insert(0, creatorDto); // Вставляем создателя в начало списка
                }
            }
        }

        return Result<IEnumerable<ProjectMemberDto>>.Success(memberDtos);
    }
} 