namespace Ediki.Api.DTOs.In;

public interface IRequestIn<out TCommand> 
{
    TCommand Convert();
}
