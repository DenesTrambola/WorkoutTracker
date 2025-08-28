namespace WorkoutTracker.Domain.Tests.Shared.ValueObjects;

using FluentAssertions;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class DescriptionTests
{
    private readonly string _validDescriptionText = "This is a description.";

    [Fact]
    public void Create_Should_ReturnSuccess_When_TextIsNull()
    {
        // Arrange
        string? descriptionText = null;

        // Act
        Result<Description> descriptionResult = Description.Create(descriptionText);

        // Assert
        descriptionResult.IsSuccess.Should().BeTrue();
        descriptionResult.ValueOrDefault().Text.Should().BeNull();
        descriptionResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnSuccess_When_TextIsNotNull()
    {
        // Arrange
        string descriptionText = _validDescriptionText;

        // Act
        Result<Description> descriptionResult = Description.Create(descriptionText);

        // Assert
        descriptionResult.IsSuccess.Should().BeTrue();
        descriptionResult.ValueOrDefault().Text.Should().NotBeNull();
        descriptionResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnSuccess_When_TextIsNotTooLong()
    {
        // Arrange
        string descriptionText = new string('a', Description.MaxLength);

        // Act
        Result<Description> descriptionResult = Description.Create(descriptionText);

        // Assert
        descriptionResult.IsSuccess.Should().BeTrue();
        descriptionResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_TextisTooLong()
    {
        // Arrange
        string descriptionText = new string('a', Description.MaxLength + 1);

        // Act
        Result<Description> descriptionResult = Description.Create(descriptionText);

        // Assert
        descriptionResult.IsFailure.Should().BeTrue();
        descriptionResult.Errors.Should().Contain(DomainErrors.Description.TooLong);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_DescriptionIsNotNull()
    {
        // Arrange
        Result<Description> descriptionResult = Description.Create(_validDescriptionText);

        // Act
        descriptionResult = Description.EnsureNotNull(descriptionResult.ValueOrDefault());

        // Assert
        descriptionResult.IsSuccess.Should().BeTrue();
        descriptionResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_DescriptionIsNull()
    {
        // Arrange
        Description? description = null;

        // Act
        Result<Description> descriptionResult = Description.EnsureNotNull(description);

        // Assert
        descriptionResult.IsFailure.Should().BeTrue();
        descriptionResult.Errors.Should().Contain(DomainErrors.Description.Null);
    }

    [Fact]
    public void DescriptionsWithSameValues_Should_BeEqual()
    {
        // Arrange
        Description description1 = Description.Create(_validDescriptionText).ValueOrDefault();
        Description description2 = Description.Create(_validDescriptionText).ValueOrDefault();

        // Act
        bool descriptionsAreEqual = description1 == description2;

        // Assert
        descriptionsAreEqual.Should().BeTrue();
    }

    [Fact]
    public void DescriptionsWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        Description description1 = Description.Create("Good").ValueOrDefault();
        Description description2 = Description.Create("Bad").ValueOrDefault();

        // Act
        bool descriptionsAreDifferent = description1 != description2;

        // Assert
        descriptionsAreDifferent.Should().BeTrue();
    }
}
