namespace Ediki.Domain.Enums;

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string User = "User"; 
    public const string Creator = "Creator";
    
    public static List<string> GetAllRoles() => new() { Admin, User, Creator };
}
