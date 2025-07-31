namespace WorkoutTracker.Domain.Shared.Errors;

public class Error : IEquatable<Error>
{
    public string Code { get; }
    public string Message { get; }

    public static readonly Error None = new Error(string.Empty, string.Empty);

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public bool Equals(Error? other)
        => other is not null && Code == other.Code && Message == other.Message;

    public static bool operator ==(Error? left, Error? right)
        => left is not null && left.Equals(right);

    public static bool operator !=(Error? left, Error? right)
        => !(left == right);

    public override bool Equals(object? obj)
        => Equals(obj as Error);

    public override int GetHashCode()
        => HashCode.Combine(Code, Message);

    public override string ToString()
        => $"{Code}: {Message}";
}
