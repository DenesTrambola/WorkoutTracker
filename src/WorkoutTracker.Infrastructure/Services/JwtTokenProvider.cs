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

public sealed class JwtTokenProvider(JwtOptions options) : IAccessTokenProvider
{
    private readonly JwtOptions _options = options;

    public Result<AccessToken> GenerateToken(UserId userId, Email email, UserRole role)
    {
        var secretKey = _options.SecretKey;
        var expiryMinutes = _options.ExpiryMinutes;
        var issuer = _options.Issuer;
        var audience = _options.Audience;

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

            return Result.Success(new AccessToken
            {
                Token = tokenString,
                ExpiresAt = expires
            });
        }
        catch (Exception)
        {
            return Result.Failure<AccessToken>(ApplicationErrors.AccessToken.GenerationFailed);
        }
    }
}
