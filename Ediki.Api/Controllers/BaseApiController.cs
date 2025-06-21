using Ediki.Api.DTOs;
using Ediki.Api.DTOs.In;
using Ediki.Api.DTOs.Out;
using Ediki.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SummerFAFHackaton_2025.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseApiController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;

    protected async Task<IActionResult> ExecuteAsync<TRequest, TResponse, TResult>(
        IRequestIn<TRequest> requestDto)
        where TRequest : IRequest<Result<TResult>>
        where TResponse : IResponseOut<TResult>, new()
    {
        try
        {
            var command = requestDto.Convert();
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return HandleFailureResult(result);
            }

            var responseDto = new TResponse();
            var response = new BaseResponse<object?>
            {
                Success = true,
                Data = responseDto.Convert(result.Value),
                Message = "Operation completed successfully"
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    protected async Task<IActionResult> ExecuteQueryAsync<TResponse, TResult>(
        IRequest<Result<TResult>> query)
        where TResponse : IResponseOut<TResult>, new()
    {
        try
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return HandleFailureResult(result);
            }

            var responseDto = new TResponse();
            var response = new BaseResponse<object?>
            {
                Success = true,
                Data = responseDto.Convert(result.Value),
                Message = "Operation completed successfully"
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    protected async Task<IActionResult> ExecuteListQueryAsync<TResponse, TResult>(
        IRequest<Result<IEnumerable<TResult>>> query)
        where TResponse : IResponseOut<TResult>, new()
    {
        try
        {
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return HandleFailureResult(result);
            }

            var responseDto = new TResponse();
            var convertedData = result.Value.Select(item => responseDto.Convert(item)).ToList();

            var response = new BaseResponse<object?>
            {
                Success = true,
                Data = convertedData,
                Message = "Operation completed successfully"
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    private IActionResult HandleFailureResult<T>(Result<T> result)
    {
        var response = new BaseResponse<object?>
        {
            Success = false,
            Message = "Operation failed",
            Errors = result.Errors.Any() ? result.Errors : new List<string> { result.Error ?? "Unknown error" }
        };

        return BadRequest(response);
    }

    private IActionResult HandleException(Exception ex)
    {
        var response = new BaseResponse<object?>
        {
            Success = false,
            Message = "An error occurred while processing the request",
            Errors = [ex.Message]
        };

        return StatusCode(500, response);
    }
}
