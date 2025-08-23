namespace WorkoutTracker.Infrastructure.Tests;

using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Microsoft.Extensions.Options;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;
using WorkoutTracker.Infrastructure.Models;

public class JwtTokenProviderTests
{
    private const string SecretKey = "SuperSecretKey12345678910111213141516171819";
    private const int ExpiryMinutes = 60;
    private const string Issuer = "WorkoutTracker";
    private const string Audience = "WorkoutTrackerClients";

    [Fact]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Arrange
        var jwtSettings = new JwtSettings(SecretKey, ExpiryMinutes, Issuer, Audience);
        var provider = new JwtTokenProvider(Options.Create(jwtSettings));
        var userId = UserId.New().ValueOrDefault();
        var email = Email.Create("test@example.com").ValueOrDefault();
        var role = UserRole.User;

        // Act
        var result = provider.GenerateToken(userId, email, role);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault().Token.Should().NotBeNullOrEmpty();
        result.ValueOrDefault().ExpiresAt.Should().BeAfter(DateTime.UtcNow);
    }
}
