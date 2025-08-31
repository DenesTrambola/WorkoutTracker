namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Routines.Commands.Create;
using WorkoutTracker.Web.Presentation.Primitives;
using WorkoutTracker.Web.Presentation.Requests.Routines;

[Route("api/routines")]
public sealed class RoutineController(ISender sender)
    : ApiController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoutineDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateRoutineCommand
        {
            Name = request.Name,
            Description = request.Description,
            UserId = request.UserId
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), null)
            : BadRequest(new
            {
                Message = "Failed to create exercise",
                Errors = result.Errors
            });
    }
}
