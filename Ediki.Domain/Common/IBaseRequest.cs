
using MediatR;
using IRequest = Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.IRequest;

namespace Ediki.Domain.Common;

public interface IBaseRequest : IRequest
{
}

public interface IBaseRequest<out TResponse> : IRequest<TResponse>
{
}
