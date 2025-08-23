namespace WorkoutTracker.Web.Presentation.Requests;

using System.ComponentModel.DataAnnotations;

public sealed record RegisterUserRequestDto
{
    [Required]
    public string Username { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;

    [Required]
    public string FirstName { get; init; } = null!;

    [Required]
    public string LastName { get; init; } = null!;

    [Required]
    public byte Gender { get; init; }

    [Required]
    public DateOnly BirthDate { get; init; }
}
