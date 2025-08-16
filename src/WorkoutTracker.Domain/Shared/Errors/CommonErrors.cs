namespace WorkoutTracker.Domain.Shared.Errors;

public static partial class DomainErrors
{
    public static readonly Error None = new Error(
        string.Empty,
        string.Empty);

    internal static Error Empty(string name)
        => new Error(
            $"{name}.Empty",
            $"{name} cannot be empty.");

    internal static Error TooLong(string name, short maxLength)
        => new Error(
            $"{name}.TooLong",
            $"{name} exceeds the maximum allowed length ({maxLength}).");

    internal static Error InvalidFormat(string name)
        => new Error(
            $"{name}.InvalidFormat",
            $"{name} is not in a valid format.");

    internal static Error InvalidValue(string name)
        => new Error(
            $"{name}.InvalidValue",
            $"{name} does not have a valid value.");

    internal static Error AlreadyExists(string name)
        => new Error(
            $"{name}.AlreadyExists",
            $"{name} already exists.");

    internal static Error Null(string name)
        => new Error(
            $"{name}.Null",
            $"{name} cannot be null.");

    internal static Error NotFound(string name)
        => new Error(
            $"{name}.NotFound",
            $"{name} was not found.");

    internal static Error CannotRemove(string name, string from)
        => new Error(
            $"{name}.CannotRemove",
            $"Cannot remove {name} from {from}");
}
