namespace WorkoutTracker.Web.Presentation.Requests;

using System.ComponentModel.DataAnnotations;

public sealed record RegisterUserRequestDto
{
    [Required]
    public required string Username { get; init; } = null!;


    [Required]
    public required string Password { get; init; } = null!;


    [Required]
    [EmailAddress]
    public required string Email { get; init; } = null!;


    [Required]
    public required string FirstName { get; init; } = null!;


    [Required]
    public required string LastName { get; init; } = null!;


    [Required]
    public required byte Gender { get; init; }


    [Required]
    public required DateOnly BirthDate { get; init; }
}
