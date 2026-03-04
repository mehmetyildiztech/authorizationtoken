# Authorization Token API

A secure ASP.NET Core 8 Web API implementing JWT authentication with refresh tokens, built using Domain-Driven Design principles.

## Features

- ✅ **JWT Authentication** - Secure access token-based authentication
- ✅ **Refresh Tokens** - Long-lived tokens stored in HTTP-only cookies
- ✅ **Token Rotation** - Automatic refresh token rotation for enhanced security
- ✅ **ASP.NET Core Identity** - User management with password hashing
- ✅ **Entity Framework Core** - SQL Server database integration
- ✅ **Domain-Driven Design** - Clean architecture with separated concerns
- ✅ **Secure Cookie Storage** - HTTP-only, Secure, SameSite cookies

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication

## Project Structure

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server LocalDB
- Visual Studio 2022 or Visual Studio Code

### Installation

1. **Clone the repository**

2. **Configure application settings**
   
   Copy `appsettings.Development.json.template` to `appsettings.Development.json`:

3. **Update connection string**
   
   Edit `appsettings.Development.json` and update the connection string for your SQL Server instance.

4. **Set JWT Secret Key**
   
   Update the `JwtSettings:SecretKey` in `appsettings.Development.json` with a secure key (minimum 32 characters).

5. **Run database migrations**

6. **Run the application**

The API will be available at `https://localhost:5123` (or the port specified in `launchSettings.json`).

## API Endpoints

### Authentication

#### Register a new user

#### Login

#### Refresh access token

#### Revoke refresh token

### Protected Endpoints

#### Get payments (requires authentication)

## Security Features

### Password Requirements
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one non-alphanumeric character

### Token Security
- **Access Tokens**: Short-lived (1 hour by default), sent in Authorization header
- **Refresh Tokens**: Long-lived (7 days by default), stored in HTTP-only cookies
- **Token Rotation**: New refresh token issued on each refresh
- **Secure Cookies**: HttpOnly, Secure, SameSite=Strict

### Account Lockout
- Maximum 5 failed login attempts
- 5-minute lockout period

## Configuration

### JWT Settings (appsettings.json)

### Database Connection

Update the connection string in `appsettings.json`:

## Testing

Use the included `.http` file (`AuthorizationToken.http`) with Visual Studio or REST Client extension for VS Code to test the API endpoints.

## Production Deployment

### Important Security Steps

1. **Use Azure Key Vault or environment variables for secrets**
2. **Update CORS settings** in `Program.cs` for your frontend domain
3. **Use production database** - Update connection string
4. **Enable HTTPS** - Ensure certificates are properly configured
5. **Set secure cookie options** - Already configured (HttpOnly, Secure, SameSite)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

Mehmet Yildiz






