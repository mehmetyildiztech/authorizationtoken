namespace AuthorizationToken.DTOs.Request
{
    public record RefreshTokenRequest
    {
        public string Username { get; init; } = string.Empty;
    }
}
