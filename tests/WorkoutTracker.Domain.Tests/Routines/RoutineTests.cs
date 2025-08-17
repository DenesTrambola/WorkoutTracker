namespace WorkoutTracker.Domain.Tests.Routines;

using FluentAssertions;
using WorkoutTracker.Domain.Exercises.TypedIds;
using WorkoutTracker.Domain.Routines;
using WorkoutTracker.Domain.Routines.Errors;
using WorkoutTracker.Domain.Routines.TypedIds;
using WorkoutTracker.Domain.Routines.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;
using WorkoutTracker.Domain.Users.TypedIds;
using static System.Runtime.InteropServices.JavaScript.JSType;

public sealed class RoutineTests
{
    private readonly Name _validName = Name.Create("Push").ValueOrDefault();
    private readonly Description _validDescription = Description.Create(null).ValueOrDefault();
    private readonly UserId _validUserId = UserId.New().ValueOrDefault();

    [Fact]
    public void Create_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Act
        Result<Routine> routineResult = Routine.Create(
            _validName,
            _validDescription,
            _validUserId);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Name? name = null;

        // Act
        Result<Routine> routineResult = Routine.Create(
            name!,
            _validDescription,
            _validUserId);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Description? description = null;

        // Act
        Result<Routine> routineResult = Routine.Create(
            _validName,
            description!,
            _validUserId);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Description.Null);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_UserIdIsNull()
    {
        // Arrange
        UserId? userId = null;

        // Act
        Result<Routine> routineResult = Routine.Create(
            _validName,
            _validDescription,
            userId!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Users.Errors.DomainErrors.UserId.Null);
    }

    [Fact]
    public void UpdateName_Should_ReturnSuccess_When_NameIsValid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        Name newName = Name.Create("Pull").ValueOrDefault();

        // Act
        Result<Routine> routineResult = routine.UpdateName(newName);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.ValueOrDefault().Name.Should().Be(newName);
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void UpdateName_Should_ReturnFailure_When_NameIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();
        Name? newName = null;

        // Act
        Result<Routine> routineResult = routine.UpdateName(newName!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Name.Null);
    }

    [Fact]
    public void UpdateDescription_Should_ReturnSuccess_When_DescriptionIsValid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        Description newDescription = Description.Create("Description").ValueOrDefault();

        // Act
        Result<Routine> routineResult = routine.UpdateDescription(newDescription);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.ValueOrDefault().Description.Should().Be(newDescription);
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }


    [Fact]
    public void UpdateDescription_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();
        Description? newDescription = null;

        // Act
        Result<Routine> routineResult = routine.UpdateDescription(newDescription!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Description.Null);
    }

    [Fact]
    public void AddExercise_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            3, 3,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault());

        // Assert
        exerciseResult.IsSuccess.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().NotBeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        routine.RoutineExercises.Should().Contain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void AddExercise_Should_ReturnFailure_When_SetCountIsInvalid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            0, 12,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault());

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.RoutineExercise.InvalidSetCount);
        routine.RoutineExercises.Should().NotContain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void AddExercise_Should_ReturnFailure_When_RepCountIsInvalid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            3, 0,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault());

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.RoutineExercise.InvalidRepCount);
        routine.RoutineExercises.Should().NotContain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void AddExercise_Should_ReturnFailure_When_RestTimeIsInvalid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            3, 12,
            new TimeSpan(0, -1, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault());

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.RoutineExercise.InvalidRestTimeBetweenSets);
        routine.RoutineExercises.Should().NotContain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void AddExercise_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        Comment? comment = null;

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            3, 12,
            new TimeSpan(0, 2, 0),
            comment!,
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault());

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.Comment.Null);
        routine.RoutineExercises.Should().NotContain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void AddExercise_Should_ReturnFailure_When_PositionIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        ExercisePosition? position = null;

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            3, 12,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            position!,
            ExerciseId.New().ValueOrDefault());

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(DomainErrors.ExercisePosition.Null);
        routine.RoutineExercises.Should().NotContain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void AddExercise_Should_ReturnFailure_When_ExerciseIdIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        ExerciseId? exerciseId = null;

        // Act
        Result<RoutineExercise> exerciseResult = routine.AddExercise(
            3, 12,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            exerciseId!);

        // Assert
        exerciseResult.IsFailure.Should().BeTrue();
        exerciseResult.ValueOrDefault().Should().BeNull();
        exerciseResult.Errors.Should().Contain(Domain.Exercises.Errors.DomainErrors.ExerciseId.Null);
        routine.RoutineExercises.Should().NotContain(exerciseResult.ValueOrDefault());
    }

    [Fact]
    public void RemoveExercise_Should_ReturnSuccess_When_ValuesAreValid()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        RoutineExercise exercise = routine.AddExercise(
            3, 12,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();

        // Act
        Result<Routine> routineResult = routine.RemoveExercise(exercise.Id);

        // Assert
        routineResult.IsSuccess.Should().BeTrue();
        routineResult.ValueOrDefault().Should().NotBeNull();
        routineResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        routine.RoutineExercises.Should().NotContain(exercise);
    }

    [Fact]
    public void RemoveExercise_Should_ReturnFailure_When_RoutineExerciseIdIsNull()
    {
        // Arrange
        Routine routine = Routine.Create(
            _validName,
            _validDescription,
            _validUserId)
            .ValueOrDefault();
        RoutineExerciseId? exerciseId = null;

        // Act
        Result<Routine> routineResult = routine.RemoveExercise(exerciseId!);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(DomainErrors.RoutineExerciseId.Null);
    }

    [Fact]
    public void RemoveExercise_Should_ReturnFailure_When_RoutineExerciseIsNotFound()
    {
        // Arrange
        Routine routine = Routine.Create(
           _validName,
           _validDescription,
           _validUserId)
           .ValueOrDefault();
        RoutineExercise exercise = routine.AddExercise(
            3, 12,
            new TimeSpan(0, 2, 0),
            Comment.Create(null).ValueOrDefault(),
            ExercisePosition.Create(1).ValueOrDefault(),
            ExerciseId.New().ValueOrDefault())
            .ValueOrDefault();
        Result<Routine> routineResult = routine.RemoveExercise(exercise.Id);

        // Act
        routineResult = routine.RemoveExercise(exercise.Id);

        // Assert
        routineResult.IsFailure.Should().BeTrue();
        routineResult.ValueOrDefault().Should().BeNull();
        routineResult.Errors.Should().Contain(DomainErrors.RoutineExercise.NotFound);
        routine.RoutineExercises.Should().NotContain(exercise);
    }

    [Fact]
    public void RoutinesWithSameValues_ShouldNot_BeEqual()
    {
        // Arrange
        Routine routine1 = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();
        Routine routine2 = Routine.Create(
            _validName,
            _validDescription,
            _validUserId).ValueOrDefault();

        // Act
        bool routinesAreDifferent = routine1 != routine2;

        // Assert
        routinesAreDifferent.Should().BeTrue();
    }
}
