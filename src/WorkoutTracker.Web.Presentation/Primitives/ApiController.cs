namespace WorkoutTracker.Web.Presentation.Primitives;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }
}
