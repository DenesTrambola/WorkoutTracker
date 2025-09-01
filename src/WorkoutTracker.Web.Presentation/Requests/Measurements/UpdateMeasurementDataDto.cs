namespace WorkoutTracker.Web.Presentation.Requests.Measurements;

public sealed record UpdateMeasurementDataDto
{
    public float? Value { get; init; }

    public DateTime? MeasuredOn { get; init; }

    public string? Comment { get; init; }

    public Guid? MeasurementId { get; init; }
}
