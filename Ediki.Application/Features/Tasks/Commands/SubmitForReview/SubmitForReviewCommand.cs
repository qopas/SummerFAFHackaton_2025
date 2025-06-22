using Ediki.Application.Features.Tasks.DTOs;
using Ediki.Domain.Common;
using MediatR;

namespace Ediki.Application.Features.Tasks.Commands.SubmitForReview;

public class SubmitForReviewCommand : IRequest<Result<TaskDto>>
{
    public string TaskId { get; set; } = string.Empty;
    public string? ReviewNotes { get; set; }
} 