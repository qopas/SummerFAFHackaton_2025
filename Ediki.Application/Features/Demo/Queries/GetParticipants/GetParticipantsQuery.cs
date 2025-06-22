using Ediki.Domain.Common;
using Ediki.Application.Features.Demo.DTOs;
using MediatR;

namespace Ediki.Application.Features.Demo.Queries.GetParticipants;

public class GetParticipantsQuery : IRequest<Result<List<DemoParticipantDto>>>
{
    public string SessionId { get; set; } = string.Empty;
} 