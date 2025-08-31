namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Exercises.Commands.Create;
using WorkoutTracker.Application.Exercises.Queries.GetAll;
using WorkoutTracker.Application.Exercises.Queries.GetById;
using WorkoutTracker.Web.Presentation.Primitives;
using WorkoutTracker.Web.Presentation.Requests.Exercises;

[Route("api/exercises")]
public sealed class ExerciseController(ISender sender)
    : ApiController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateExerciseDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateExerciseCommand
        {
            Name = request.Name,
            TargetMuscle = request.TargetMuscle,
            IsPublic = request.IsPublic,
            UserId = request.UserId
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
             ? CreatedAtAction(nameof(Create), null)
             : BadRequest(new
             {
                 Message = "Exercise creation failed",
                 Errors = result.Errors
             });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllExercisesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
             ? Ok(result.ValueOrDefault())
             : BadRequest(new
             {
                 Message = "Failed to retrieve exercises",
                 Errors = result.Errors
             });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetExerciseByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve measurement",
                Errors = result.Errors
            });
    }
}
