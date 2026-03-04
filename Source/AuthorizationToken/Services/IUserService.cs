using AuthorizationToken.Domain.Entities;

namespace AuthorizationToken.Services
{
    public interface IUserService
    {
        Task<(bool Success, ApplicationUser? User)> ValidateUserAsync(string username, string password);
        Task<(bool Success, string[] Errors)> RegisterUserAsync(string username, string email, string password);
        Task<ApplicationUser?> GetUserByUsernameAsync(string username);
    }
}