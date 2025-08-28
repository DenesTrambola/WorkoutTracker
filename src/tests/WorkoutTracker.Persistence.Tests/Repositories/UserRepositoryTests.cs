namespace WorkoutTracker.Persistence.Tests.Repositories;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.ValueObjects;
using WorkoutTracker.Persistence.Repositories;

public sealed class UserRepositoryTests
{
    [Fact]
    public async Task AddAsync_Should_AddUserToDatabase_When_UserIsValid()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        // Act
        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        var operationResult = await unitOfWork.Users.AddAsync(user);
        saveResult = await unitOfWork.SaveChangesAsync();

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        (await unitOfWork.Users.GetAllAsync()).ValueOrDefault().Should().Contain(user);
        (await unitOfWork.Users.GetByIdAsync(user.Id)).ValueOrDefault().Should().Be(user);
    }

    [Fact]
    public async Task RemoveAsync_Should_AddUserToDatabase_When_UserIsValid()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        // Act
        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        var removeResult = await addResult.OnSuccessAsync(
            async u => await unitOfWork.Users.RemoveAsync(user.Id));
        saveResult = await unitOfWork.SaveChangesAsync();

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        removeResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        (await unitOfWork.Users.GetAllAsync()).ValueOrDefault().Should().NotContain(user);
        (await unitOfWork.Users.GetByIdAsync(user.Id)).ValueOrDefault().Should().NotBe(user);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllUsers_When_UsersExist()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        // Act
        var getAllResult = await unitOfWork.Users.GetAllAsync();

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        getAllResult.IsSuccess.Should().BeTrue();
        getAllResult.ValueOrDefault().Should().NotBeEmpty();
        getAllResult.ValueOrDefault().Should().Contain(user);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        // Act
        var getByIdResult = await unitOfWork.Users.GetByIdAsync(user.Id);

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        getByIdResult.IsSuccess.Should().BeTrue();
        getByIdResult.ValueOrDefault().Should().Be(user);
    }

    [Fact]
    public async Task GetByUsernameAsync_Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        // Act
        var getByUsernameResult = await unitOfWork.Users.GetByUsernameAsync(user.Username);

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        getByUsernameResult.IsSuccess.Should().BeTrue();
        getByUsernameResult.ValueOrDefault().Should().Be(user);
    }

    [Fact]
    public async Task ValidateEmailUniqueness_Should_ReturnFailure_When_EmailIsNotUnique()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        // Act
        var emailUniquenessResult = await unitOfWork.Users.ValidateEmailUniqueness(user.Email);

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        emailUniquenessResult.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateEmailUniqueness_Should_ReturnSuccess_When_EmailIsUnique()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        // Act
        var emailUniquenessResult = await unitOfWork.Users.ValidateEmailUniqueness(user.Email);

        // Assert
        emailUniquenessResult.IsSuccess.Should().BeTrue();
        emailUniquenessResult.ValueOrDefault().Should().Be(user.Email);
    }

    [Fact]
    public async Task ValidateUsernameUniqueness_Should_ReturnSuccess_When_UsernameIsUnique()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        // Act
        var usernameUniquenessResult = await unitOfWork.Users.ValidateUsernameUniqueness(user.Username);

        // Assert
        usernameUniquenessResult.IsSuccess.Should().BeTrue();
        usernameUniquenessResult.ValueOrDefault().Should().Be(user.Username);
    }

    [Fact]
    public async Task ValidateUsernameUniqueness_Should_ReturnFailure_When_UsernameIsNotUnique()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        await context.Database.EnsureCreatedAsync();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var repository = new UserRepository(context);
        var unitOfWork = new UnitOfWork(context, repository);

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deinesh", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        var addResult = await unitOfWork.Users.AddAsync(user);
        var saveResult = await unitOfWork.SaveChangesAsync();

        // Act
        var usernameUniquenessResult = await unitOfWork.Users.ValidateUsernameUniqueness(user.Username);

        // Assert
        addResult.IsSuccess.Should().BeTrue();
        saveResult.Should().BeGreaterThan(0);
        usernameUniquenessResult.IsFailure.Should().BeTrue();
    }
}
