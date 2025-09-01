namespace WorkoutTracker.Web.Presentation.Requests.Routines;

public sealed record UpdateRoutineDto
{
    public string? Name { get; init; }

    public string? Description { get; init; }

    public Guid? UserId { get; init; }
}
