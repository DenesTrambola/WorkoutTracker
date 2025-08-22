namespace WorkoutTracker.Persistence.Tests;

using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Domain.Exercises;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Measurements;
using WorkoutTracker.Domain.Measurements.Enums;
using WorkoutTracker.Domain.Measurements.ValueObjects;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users;
using WorkoutTracker.Domain.Users.TypedIds;
using WorkoutTracker.Domain.Users.ValueObjects;

public class AppDbContextTests
{
    [Fact]
    public void AppDbContext_Should_SetupConnection()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);

        // Act
        context.Database.EnsureCreated();

        // Assert
        context.Database.CanConnect().Should().BeTrue();
        context.Model.GetEntityTypes().Should().NotBeEmpty();
    }

    [Fact]
    public void AppDbContext_Should_AddExercise()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var exercise = Exercise.Create(
            Name.Create("Push-up").ValueOrDefault(),
            TargetMuscle.Create("Chest").ValueOrDefault(),
            Visibility.Create(false).ValueOrDefault(),
            UserId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        context.Add(exercise);
        context.SaveChanges();

        // Assert
        context.Exercises.Should().Contain(exercise);
    }

    [Fact]
    public void AppDbContext_Should_AddMeasurement()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var measurement = Measurement.Create(
            Name.Create("Right Arm").ValueOrDefault(),
            Description.Create(null).ValueOrDefault(),
            MeasurementUnit.Centimeter,
            UserId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        context.Add(measurement);
        context.SaveChanges();

        // Assert
        context.Measurements.Should().Contain(measurement);
    }

    [Fact]
    public void AppDbContext_Should_AddMeasurementData()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var measurement = Measurement.Create(
            Name.Create("Right Arm").ValueOrDefault(),
            Description.Create(null).ValueOrDefault(),
            MeasurementUnit.Centimeter,
            UserId.New().ValueOrDefault())
            .ValueOrDefault();
        var measurementData = measurement.AddData(
            MeasurementDataValue.Create(1).ValueOrDefault(),
            DateTime.UtcNow,
            Comment.Create(null).ValueOrDefault())
            .ValueOrDefault();

        // Act
        context.Add(measurement);
        context.SaveChanges();

        // Assert
        context.Measurements.Should().Contain(measurement);
        context.MeasurementData.Should().Contain(measurementData);
    }

    [Fact]
    public void AppDbContext_Should_AddRoutine()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var routine = Routine.Create(
            Name.Create("Push").ValueOrDefault(),
            Description.Create(null).ValueOrDefault(),
            UserId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        context.Add(routine);
        context.SaveChanges();

        // Assert
        context.Routines.Should().Contain(routine);
    }

    [Fact]
    public void AppDbContext_Should_AddRoutineExercise()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var routine = Routine.Create(
            Name.Create("Push").ValueOrDefault(),
            Description.Create(null).ValueOrDefault(),
            UserId.New().ValueOrDefault())
            .ValueOrDefault();
        var routineExercise = routine.AddExercise(
            3, 12,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        context.Add(routine);
        context.SaveChanges();

        // Assert
        context.Routines.Should().Contain(routine);
        context.RoutineExercises.Should().Contain(routineExercise);
    }

    [Fact]
    public void AppDbContext_Should_AddUser()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deines", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();

        // Act
        context.Add(user);
        context.SaveChanges();

        // Assert
        context.Users.Should().Contain(user);
    }

    [Fact]
    public void AppDbContext_Should_AddWorkout()
    {
        // Arrange
        using var context = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("WorkoutTrackerTestDb")
                .Options);
        context.Database.EnsureCreated();
        context.Model.GetEntityTypes().Should().NotBeEmpty();

        var user = User.Create(
            Username.Create("denestrambola").ValueOrDefault(),
            PasswordHash.Create("Pa$$word123").ValueOrDefault(),
            Email.Create("tramboladenes@gmail.com").ValueOrDefault(),
            FullName.Create("Deines", "Trombola").ValueOrDefault(),
            0, 0, DateOnly.FromDateTime(DateTime.UtcNow))
            .ValueOrDefault();
        var workout = user.AddWorkout(
            DateTime.UtcNow, DateTime.UtcNow,
            TimeSpan.Zero,
            Comment.Create(null).ValueOrDefault(),
            RoutineId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        context.Add(user);
        context.SaveChanges();

        // Assert
        context.Users.Should().Contain(user);
        context.Workouts.Should().Contain(workout);
    }
}
