namespace WorkoutTracker.Infrastructure.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WorkoutTracker.Application.Users.Errors;
using WorkoutTracker.Application.Users.Models;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;
using WorkoutTracker.Infrastructure.Models;

public sealed class JwtTokenProvider(IOptions<JwtSettings> jwtSettings) : IAccessTokenProvider
{
    private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings;

    public Result<AccessToken> GenerateToken(UserId userId, Email email, UserRole role)
    {
        var secretKey = _jwtSettings.Value.SecretKey;
        var expiryMinutes = _jwtSettings.Value.ExpiryMinutes;
        var issuer = _jwtSettings.Value.Issuer;
        var audience = _jwtSettings.Value.Audience;

        try
        {
            var claims = new[]
            {
                new Claim(
                    System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, userId.IdValue.ToString()),
                new Claim(
                    System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, email.Value),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Result.Success(new AccessToken(tokenString, expires));
        }
        catch (Exception)
        {
            return Result.Failure<AccessToken>(ApplicationErrors.AccessToken.GenerationFailed);
        }
    }
}
