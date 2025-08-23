namespace WorkoutTracker.Web.Presentation.Requests;

using System.ComponentModel.DataAnnotations;

public class LoginRequestDto
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
}
