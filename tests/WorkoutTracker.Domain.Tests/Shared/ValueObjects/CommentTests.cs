namespace WorkoutTracker.Domain.Tests.Shared.ValueObjects;

using System.Reflection;
using FluentAssertions;
using WorkoutTracker.Domain.Shared.Errors;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Domain.Shared.ValueObjects;

public sealed class CommentTests
{
    private readonly string _validCommentText = "This is a comment.";

    [Fact]
    public void Create_Should_ReturnSuccess_When_TextIsNull()
    {
        // Arrange
        string? commentText = null;

        // Act
        var commentResult = Comment.Create(commentText);

        // Assert
        commentResult.IsSuccess.Should().BeTrue();
        commentResult.ValueOrDefault().Text.Should().BeNull();
        commentResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnSuccess_When_TextIsNotNull()
    {
        // Arrange
        var commentText = _validCommentText;

        // Act
        var commentResult = Comment.Create(commentText);

        // Assert
        commentResult.IsSuccess.Should().BeTrue();
        commentResult.ValueOrDefault().Text.Should().NotBeNull();
        commentResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnSuccess_When_TextIsNotTooLong()
    {
        // Arrange
        var commentText = new string('a', Comment.MaxLength);

        // Act
        var commentResult = Comment.Create(commentText);

        // Assert
        commentResult.IsSuccess.Should().BeTrue();
        commentResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_TextIsTooLong()
    {
        // Arrange
        var commentText = new string('a', Comment.MaxLength + 1);

        // Act
        var commentResult = Comment.Create(commentText);

        // Assert
        commentResult.IsFailure.Should().BeTrue();
        commentResult.Errors.Should().Contain(DomainErrors.Comment.TooLong);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnSuccess_When_CommentIsNotNull()
    {
        // Arrange
        var commentResult = Comment.Create(_validCommentText);

        // Act
        commentResult = Comment.EnsureNotNull(commentResult.ValueOrDefault());

        // Assert
        commentResult.IsSuccess.Should().BeTrue();
        commentResult.Errors.Should().Contain(Domain.Shared.Errors.DomainErrors.None);
    }

    [Fact]
    public void EnsureNotNull_Should_ReturnFailure_When_CommentIsNull()
    {
        // Arrange
        Comment? comment = null;

        // Act
        var commentResult = Comment.EnsureNotNull(comment);

        // Assert
        commentResult.IsFailure.Should().BeTrue();
        commentResult.Errors.Should().Contain(DomainErrors.Comment.Null);
    }

    [Fact]
    public void CommentsWithSameValues_Should_BeEqual()
    {
        // Arrange
        Comment comment1 = Comment.Create(_validCommentText).ValueOrDefault();
        Comment comment2 = Comment.Create(_validCommentText).ValueOrDefault();

        // Act
        bool commentsAreEqual = comment1 == comment2;

        // Assert
        commentsAreEqual.Should().BeTrue();
    }

    [Fact]
    public void CommentsWithDifferentValues_ShouldNot_BeEqual()
    {
        // Arrange
        Comment comment1 = Comment.Create("First").ValueOrDefault();
        Comment comment2 = Comment.Create("Second").ValueOrDefault();

        // Act
        bool commentsAreDifferent = comment1 != comment2;

        // Assert
        commentsAreDifferent.Should().BeTrue();
    }
}
