namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Users.Commands.Delete;
using WorkoutTracker.Application.Users.Commands.Update;
using WorkoutTracker.Application.Users.Queries.GetAll;
using WorkoutTracker.Application.Users.Queries.GetById;
using WorkoutTracker.Web.Presentation.Primitives;
using WorkoutTracker.Web.Presentation.Requests.Users;

[Route("api/users")]
public sealed class UserController(ISender sender)
    : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve users",
                Errors = result.Errors
            });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve user",
                Errors = result.Errors
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateUserDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateUserCommand
        {
            Id = id,
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            Role = request.Role,
            BirthDate = request.BirthDate
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new
            {
                Message = "Failed to modify user",
                Error = result.Errors
            });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteUserCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new
            {
                Message = "Failed to delete user",
                Error = result.Errors
            });
    }
}
