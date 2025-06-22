using Ediki.Domain.Common;
using Ediki.Application.Features.Demo.DTOs;
using Ediki.Domain.Enums;
using MediatR;

namespace Ediki.Application.Features.Demo.Queries.GetDemoSessions;

public class GetDemoSessionsQuery : IRequest<Result<List<DemoSessionDto>>>
{
    public DemoSessionStatus? Status { get; set; }
    public string? UserId { get; set; }
    public bool? IsPublic { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
} 