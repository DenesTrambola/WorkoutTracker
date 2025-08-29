namespace WorkoutTracker.Application.Users.Commands.Login;

using System.Diagnostics.CodeAnalysis;
using MediatR;
using WorkoutTracker.Application.Shared.Primitives.Messaging;
using WorkoutTracker.Application.Users.Models;
using WorkoutTracker.Application.Users.Primitives;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.Enums;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenProvider accessTokenProvider)
    {
        _accessTokenProvider = accessTokenProvider;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<LoginResponse>> Handle(
        [NotNull] LoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var usernameResult = Username.Create(request.Username);
        var passwordResult = Password.Create(request.Password);
        
        var userResult = await Result.Combine(
            usernameResult, passwordResult)
            .OnSuccessAsync(async () =>
            await GetUserByUsernameAsync(usernameResult.ValueOrDefault(), cancellationToken));

        var verifyPasswordResult = await userResult.OnSuccessAsync(
            async u => await VerifyPasswordAsync(
                passwordResult.ValueOrDefault(), u.PasswordHash));
        
        var tokenResult = Result.Combine(
            verifyPasswordResult)
            .OnSuccess(() =>
            GenerateAccessToken(
                userResult.ValueOrDefault().Id,
                userResult.ValueOrDefault().Email,
                userResult.ValueOrDefault().Role));
        
        return tokenResult.Map(t =>
        {
            var user = userResult.ValueOrDefault();
            
            return new LoginResponse
            {
                UserId = user.Id.IdValue,
                Username = user.Username.Value,
                Email = user.Email.Value,
                Token = t.Token,
                ExpiresAt = t.ExpiresAt
            };
            });
    }

    private async Task<Result<User>> GetUserByUsernameAsync(
        Username username,
        CancellationToken cancellationToken)
    {
        return await _userRepository.GetByUsernameAsync(username, cancellationToken);
    }

    private async Task<Result> VerifyPasswordAsync(Password password, PasswordHash passwordHash)
    {
        return await _passwordHasher.VerifyAsync(password, passwordHash);
    }

    private Result<AccessToken> GenerateAccessToken(UserId userId, Email email, UserRole role)
    {
        return _accessTokenProvider.GenerateToken(userId, email, role);
    }
}
