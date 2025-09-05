namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Routines.Commands.Create;
using WorkoutTracker.Application.Routines.Commands.CreateExercise;
using WorkoutTracker.Application.Routines.Commands.Delete;
using WorkoutTracker.Application.Routines.Commands.DeleteExercise;
using WorkoutTracker.Application.Routines.Commands.ReorderExercises;
using WorkoutTracker.Application.Routines.Commands.Update;
using WorkoutTracker.Application.Routines.Commands.UpdateExercise;
using WorkoutTracker.Application.Routines.Queries.GetAll;
using WorkoutTracker.Application.Routines.Queries.GetAllExercises;
using WorkoutTracker.Application.Routines.Queries.GetById;
using WorkoutTracker.Application.Routines.Queries.GetExerciseById;
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
            ? Ok()
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

    [HttpPost("exercises")]
    public async Task<IActionResult> AddExercise(
        [FromBody] CreateRoutineExerciseDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateRoutineExerciseCommand
        {
            SetCount = request.SetCount,
            RepCount = request.RepCount,
            RestTimeBetweenSets = request.RestTimeBetweenSets,
            Comment = request.Comment,
            RoutineId = request.RoutineId,
            ExerciseId = request.ExerciseId
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(AddExercise), null)
            : BadRequest(new
            {
                Message = "Failed to add exercise to routine",
                Errors = result.Errors
            });
    }

    [HttpGet("exercises")]
    public async Task<IActionResult> GetAllExercises(
        [FromQuery] GetAllRoutineExercisesQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve exercises from routine",
                Errors = result.Errors
            });
    }

    [HttpGet("exercises/{id:guid}")]
    public async Task<IActionResult> GetExerciseById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetRoutineExerciseByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to retrieve exercise from routine",
                Errors = result.Errors
            });
    }

    [HttpPut("exercises/{id:guid}")]
    public async Task<IActionResult> UpdateExercise(
        Guid id,
        [FromBody] UpdateRoutineExerciseDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateRoutineExerciseCommand
        {
            Id = id,
            SetCount = request.SetCount,
            RepCount = request.RepCount,
            RestTimeBetweenSets = request.RestTimeBetweenSets,
            Comment = request.Comment,
            Position = request.Position,
            RoutineId = request.RoutineId,
            ExerciseId = request.ExerciseId
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new
            {
                Message = "Failed to update exercise in routine",
                Errors = result.Errors
            });
    }

    [HttpDelete("exercises/{id:guid}")]
    public async Task<IActionResult> RemoveExercise(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteRoutineExerciseCommand(id);

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new
            {
                Message = "Failed to remove exercise from routine",
                Errors = result.Errors
            });
    }

    [HttpPut("{id:guid}/reorder")]
    public async Task<IActionResult> ReorderExercises(
        Guid id,
        [FromBody] ReorderExerciseInRoutineDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new ReorderExercisesInRoutineCommand
        {
            RoutineId = id,
            ExerciseIdsInOrder = request.ExerciseIds
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(new
            {
                Message = "Failed to reorder exercise in routine",
                Errors = result.Errors
            });
    }
}
