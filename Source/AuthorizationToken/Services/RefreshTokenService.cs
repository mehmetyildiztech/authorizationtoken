using System.Security.Cryptography;
using AuthorizationToken.Domain.Entities;
using AuthorizationToken.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationToken.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task SaveRefreshTokenAsync(string userId, string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"] ?? "7");

            // Remove old refresh tokens for this user (optional: keep last N tokens)
            var oldTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(oldTokens);

            // Add new refresh token
            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(expirationDays),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(tokenEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            var tokenEntity = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => 
                    rt.UserId == userId && 
                    rt.Token == refreshToken);

            if (tokenEntity == null)
            {
                return false;
            }

            // Use domain method to check validity
            if (!tokenEntity.IsValid())
            {
                // Clean up expired/revoked token
                _context.RefreshTokens.Remove(tokenEntity);
                await _context.SaveChangesAsync();
                return false;
            }

            return true;
        }

        public async Task RemoveRefreshTokenAsync(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.Revoke(); // Use domain method
            }

            await _context.SaveChangesAsync();
        }
    }
}