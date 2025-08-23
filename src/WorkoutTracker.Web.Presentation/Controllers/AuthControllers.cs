namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Users.Commands.Login;
using WorkoutTracker.Application.Users.Commands.RegisterUser;
using WorkoutTracker.Domain.Shared.Results;
using WorkoutTracker.Web.Presentation.Primitives;
using WorkoutTracker.Web.Presentation.Requests;

[Route("api/auth")]
public sealed class AuthControllers : ApiController
{
    public AuthControllers(ISender sender)
        : base(sender)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            BirthDate = request.BirthDate
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Register), null) 
            : BadRequest(new
            {
                Message = "Registration failed",
                Errors = result.Errors
            });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand
        {
            Username = request.Username,
            Password = request.Password
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault()) 
            : BadRequest(new
            {
                Message = "Login failed",
                Errors = result.Errors
            });
    }
}
