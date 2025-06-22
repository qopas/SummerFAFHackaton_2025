using Ediki.Domain.Common;
using Ediki.Application.Features.Demo.DTOs;
using MediatR;

namespace Ediki.Application.Features.Demo.Queries.GetDemoSession;

public class GetDemoSessionQuery : IRequest<Result<DemoSessionDto>>
{
    public string Id { get; set; } = string.Empty;
} 