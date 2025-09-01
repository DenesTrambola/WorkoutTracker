namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Users.Queries.GetAll;
using WorkoutTracker.Application.Users.Queries.GetById;
using WorkoutTracker.Web.Presentation.Primitives;

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
}
