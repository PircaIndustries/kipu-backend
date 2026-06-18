namespace Kipu.API.Budget.Domain.Model.ValueObjects;

/// <summary>
/// Value Object representing a validated activity name for a budget item.
/// </summary>
public sealed record ActivityName
{
    public string Value { get; }

    public ActivityName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Activity name cannot be null, empty or whitespace.");

        if (value.Length > 150)
            throw new ArgumentException("Activity name cannot exceed 150 characters.");

        Value = value.Trim();
    }

    public override string ToString() => Value;
}