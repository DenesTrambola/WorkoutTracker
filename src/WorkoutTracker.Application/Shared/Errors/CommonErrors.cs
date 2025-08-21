namespace WorkoutTracker.Application.Shared.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    internal static Error AlreadyExists(string name)
        => new Error(
            $"{name}.AlreadyExists",
            $"{name} is already exists.");

    internal static Error AddingToDatabaseFailed(string name)
        => new Error(
            $"{name}.CannotAddToDatabase",
            $"An error occured while adding {name} into the database.");

    internal static Error NotFound(string name)
        => new Error(
            $"{name}.NotFound",
            $"{name} was not found.");
}
