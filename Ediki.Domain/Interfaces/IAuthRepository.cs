using Ediki.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Ediki.Domain.Interfaces;

public interface IAuthRepository
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
    Task<bool> CreateUserAsync(ApplicationUser user, string password);
    Task AddToRoleAsync(ApplicationUser user, string role);
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    Task<string> GenerateRefreshTokenAsync(ApplicationUser user);
} 