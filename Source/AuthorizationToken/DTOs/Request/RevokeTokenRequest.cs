namespace AuthorizationToken.DTOs.Request
{
    public record RevokeTokenRequest
    {
        public string Username { get; init; } = string.Empty;
    }
}
