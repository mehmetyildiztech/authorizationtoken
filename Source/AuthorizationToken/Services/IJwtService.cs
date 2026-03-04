namespace AuthorizationToken.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(string username);
        int GetAccessTokenExpirationSeconds();
    }
}