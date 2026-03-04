namespace AuthorizationToken.Services
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        Task SaveRefreshTokenAsync(string userId, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);
        Task RemoveRefreshTokenAsync(string userId);
        Task RevokeAllUserTokensAsync(string userId);
    }
}