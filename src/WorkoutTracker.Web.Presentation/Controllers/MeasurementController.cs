namespace WorkoutTracker.Web.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Application.Measurements.Commands.CreateMeasurement;
using WorkoutTracker.Web.Presentation.Primitives;
using WorkoutTracker.Web.Presentation.Requests;

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
}
