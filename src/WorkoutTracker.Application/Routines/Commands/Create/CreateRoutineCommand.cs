namespace WorkoutTracker.Application.Routines.Commands.Create;

using WorkoutTracker.Application.Shared.Primitives.Messaging;

public sealed record CreateRoutineCommand
    : ICommand
{
    public required string Name { get; init; }

    public required string Description { get; init; }

    public required Guid UserId { get; init; }
}
