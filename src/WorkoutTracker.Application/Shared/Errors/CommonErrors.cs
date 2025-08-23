namespace WorkoutTracker.Application.Shared.Errors;

using WorkoutTracker.Domain.Shared.Errors;

public static partial class ApplicationErrors
{
    internal static Error NotFound(string name)
        => new Error(
            $"{name}.NotFound",
            $"{name} was not found.");

    internal static Error CannotAddToDatabase(string name)
        => new Error(
            $"{name}.CannotAddToDatabase",
            $"An error occured while adding {name} into the database.");

    internal static Error CannotDeleteFromDatabase(string name)
        => new Error(
            $"{name}.CannotDeleteFromDatabase",
            $"An error occured while deleting {name} from the database.");

    internal static Error Taken(string name)
        => new Error(
            $"{name}.AlreadyInUse",
            $"The specified {name} is already in use.");
}
