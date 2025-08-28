namespace WorkoutTracker.Domain.Shared.Errors;

public class Error : IEquatable<Error>
{
    public string Code { get; }
    public string Description { get; }

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public bool Equals(Error? other)
        => other is not null && Code == other.Code && Description == other.Description;

    public static bool operator ==(Error? left, Error? right)
        => left is not null && left.Equals(right);

    public static bool operator !=(Error? left, Error? right)
        => !(left == right);

    public override bool Equals(object? obj)
        => Equals(obj as Error);

    public override int GetHashCode()
        => HashCode.Combine(Code, Description);

    public override string ToString()
        => $"{Code}: {Description}";
}
