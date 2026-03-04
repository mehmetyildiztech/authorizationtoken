namespace AuthorizationToken.DTOs.Request
{
    public record LoginRequest
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
