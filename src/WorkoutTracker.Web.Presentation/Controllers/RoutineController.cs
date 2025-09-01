namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Routines.Commands.Create;
using WorkoutTracker.Application.Routines.Commands.Delete;
using WorkoutTracker.Application.Routines.Commands.Update;
using WorkoutTracker.Application.Routines.Queries.GetAll;
using WorkoutTracker.Application.Routines.Queries.GetById;
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
                Message = "Failed to create routine",
                Errors = result.Errors
            });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllRoutinesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve routines",
                Errors = result.Errors
            });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetRoutineByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve routine",
                Errors = result.Errors
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateRoutineDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateRoutineCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            UserId = request.UserId,
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to modify routine",
                Error = result.Errors
            });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteRoutineCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new
            {
                Message = "Failed to delete routine",
                Error = result.Errors
            });
    }
}
