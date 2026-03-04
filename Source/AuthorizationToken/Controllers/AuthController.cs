using Microsoft.AspNetCore.Mvc;
using AuthorizationToken.Services;
using AuthorizationToken.DTOs.Request;
using Microsoft.AspNetCore.CookiePolicy;

namespace AuthorizationToken.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;

        public AuthController(
            IRefreshTokenService refreshTokenService, 
            IJwtService jwtService,
            IUserService userService)
        {
            _refreshTokenService = refreshTokenService;
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var (success, errors) = await _userService.RegisterUserAsync(
                request.Username, 
                request.Email, 
                request.Password);

            if (success)
            {
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(new { message = "Registration failed", errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (success, user) = await _userService.ValidateUserAsync(request.Username, request.Password);

            if (!success || user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var accessToken = _jwtService.GenerateAccessToken(user.UserName!);
            var refreshToken = _refreshTokenService.GenerateRefreshToken();
            
            await _refreshTokenService.SaveRefreshTokenAsync(user.Id, refreshToken);

            // Store refresh token in HTTP-only cookie
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new 
            { 
                accessToken,
                expiresIn = _jwtService.GetAccessTokenExpirationSeconds()
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            // Get refresh token from cookie
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized(new { message = "Refresh token not found" });
            }

            // Verify user exists
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }

            if (!await _refreshTokenService.ValidateRefreshTokenAsync(user.Id, refreshToken))
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            // Generate new access token
            var newAccessToken = _jwtService.GenerateAccessToken(request.Username);
            
            // Generate new refresh token (refresh token rotation)
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();
            await _refreshTokenService.SaveRefreshTokenAsync(user.Id, newRefreshToken);

            // Update refresh token in cookie
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new 
            { 
                accessToken = newAccessToken,
                expiresIn = _jwtService.GetAccessTokenExpirationSeconds()
            });
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequest request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            await _refreshTokenService.RemoveRefreshTokenAsync(user.Id);

            // Delete refresh token cookie
            Response.Cookies.Delete("refreshToken");

            return Ok(new { message = "Token revoked successfully" });
        }
    }
}