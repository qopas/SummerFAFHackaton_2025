using Ediki.Application.Features.Teams.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Teams.Queries.GetTeamById;

public class GetTeamByIdQuery : IRequest<Result<TeamDto>>
{
    public string Id { get; }

    public GetTeamByIdQuery(string id)
    {
        Id = id;
    }
}