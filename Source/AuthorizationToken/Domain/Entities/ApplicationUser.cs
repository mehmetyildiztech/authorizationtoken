using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AuthorizationToken.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom properties here if needed
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for refresh tokens
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}