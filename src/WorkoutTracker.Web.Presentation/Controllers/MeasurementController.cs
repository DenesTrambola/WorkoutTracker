namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Measurements.Commands.Create;
using WorkoutTracker.Application.Measurements.Commands.Update;
using WorkoutTracker.Application.Measurements.Queries.GetAll;
using WorkoutTracker.Application.Measurements.Queries.GetById;
using WorkoutTracker.Web.Presentation.Primitives;
using WorkoutTracker.Web.Presentation.Requests.Measurements;

[Route("api/measurements")]
public sealed class MeasurementController(ISender sender)
    : ApiController(sender)
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMeasurementDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateMeasurementCommand
        {
            Name = request.Name,
            Description = request.Description,
            Unit = request.Unit,
            UserId = request.UserId
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(Create), null) 
            : BadRequest(new
            {
                Message = "Measurement creation failed",
                Errors = result.Errors
            });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllMeasurementsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault()) 
            : BadRequest(new
            {
                Message = "Failed to retrieve measurements",
                Errors = result.Errors
            });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetMeasurementByIdQuery(id);

        var result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault()) 
            : BadRequest(new
            {
                Message = "Failed to retrieve measurement",
                Errors = result.Errors
            });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateMeasurementDto request,
        CancellationToken cancellationToken = default)
    {
        var command = new UpdateMeasurementCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            Unit = request.Unit,
            UserId = request.UserId
        };

        var result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Ok(result.ValueOrDefault())
            : BadRequest(new
            {
                Message = "Failed to modify measurement",
                Error = result.Errors
            });
    }
}
