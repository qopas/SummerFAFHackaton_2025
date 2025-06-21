using Ediki.Domain.Entities;

namespace Ediki.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(ApplicationUser user);
    Task<string> GenerateRefreshToken(ApplicationUser user);
    Task<bool> ValidateAccessToken(string token);
} 