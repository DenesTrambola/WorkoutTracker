namespace WorkoutTracker.Domain.Tests.Exercises.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Exercises.Errors;
using WorkoutTracker.Domain.Exercises.ValueObjects;
using WorkoutTracker.Domain.Shared.Results;

public sealed class VisibilityTests
{
    [Fact]
    public void Create_Should_ReturnSuccess_When_ValueIsValid()
    {
        // Act
        Result<Visibility> visibilityResult = Visibility.Create(true);

        // Assert
        visibilityResult.IsSuccess.Should().BeTrue();
        visibilityResult.ValueOrDefault().Should().NotBeNull();
        visibilityResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Public_Should_ReturnVisibilityWithIsPublicTrue()
    {
        // Act
        Result<Visibility> visibilityResult = Visibility.Public();

        // Assert
        visibilityResult.IsSuccess.Should().BeTrue();
        visibilityResult.ValueOrDefault().Should().NotBeNull();
        visibilityResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        visibilityResult.ValueOrDefault().IsPublic.Should().BeTrue();
    }

    [Fact]
    public void Private_Should_ReturnVisibilityWithIsPublicFalse()
    {
        // Act
        Result<Visibility> visibilityResult = Visibility.Private();

        // Assert
        visibilityResult.IsSuccess.Should().BeTrue();
        visibilityResult.ValueOrDefault().Should().NotBeNull();
        visibilityResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
        visibilityResult.ValueOrDefault().IsPublic.Should().BeFalse();
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_VisibilityIsNotNull()
    {
        // Arrange
        Result<Visibility> visibilityResult = Visibility.Create(false);

        // Act
        visibilityResult = Visibility.EnsureNotNull(visibilityResult.ValueOrDefault());

        // Assert
        visibilityResult.IsSuccess.Should().BeTrue();
        visibilityResult.ValueOrDefault().Should().NotBeNull();
        visibilityResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_VisibilityIsNull()
    {
        // Arrange
        Visibility? visibility = null;

        // Act
        Result<Visibility> visibilityResult = Visibility.EnsureNotNull(visibility);

        // Assert
        visibilityResult.IsFailure.Should().BeTrue();
        visibilityResult.ValueOrDefault().Should().BeNull();
        visibilityResult.Errors.Should().Contain(DomainErrors.Visibility.Null);
    }

    [Fact]
    public void VisibilitiesWithSameValues_Should_BeEqual()
    {
        // Arrange
        Visibility visibility1 = Visibility.Create(false).ValueOrDefault();
        Visibility visibility2 = Visibility.Create(false).ValueOrDefault();

        // Act
        bool visibilitiesAreEqual = visibility1 == visibility2;

        // Assert
        visibilitiesAreEqual.Should().BeTrue();
    }

    [Fact]
    public void VisibilitiesWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        Visibility visibility1 = Visibility.Create(false).ValueOrDefault();
        Visibility visibility2 = Visibility.Create(true).ValueOrDefault();

        // Act
        bool visibilitiesAreDifferent = visibility1 != visibility2;

        // Assert
        visibilitiesAreDifferent.Should().BeTrue();
    }
}
