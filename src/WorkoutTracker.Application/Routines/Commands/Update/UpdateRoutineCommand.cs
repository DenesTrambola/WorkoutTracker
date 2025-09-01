namespace WorkoutTracker.Application.Routines.Commands.Update;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record UpdateRoutineCommand
    : ICommand
{
    public required Guid Id { get; init; }

    public required string? Name { get; init; }

    public required string? Description { get; init; }

    public required Guid? UserId { get; init; }
}
