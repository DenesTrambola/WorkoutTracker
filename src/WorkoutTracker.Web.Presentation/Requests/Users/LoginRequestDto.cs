namespace WorkoutTracker.Web.Presentation.Requests.Users;

using System.ComponentModel.DataAnnotations;

public class LoginRequestDto
{
    [Required]
    public required string Username { get; set; } = null!;


    [Required]
    public required string Password { get; set; } = null!;
}
