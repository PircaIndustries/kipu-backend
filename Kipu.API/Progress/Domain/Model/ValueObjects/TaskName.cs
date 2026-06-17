namespace Kipu.API.Progress.Domain.Model.ValueObjects;

public record TaskName
{
    public string Value { get; init; }

    public TaskName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Task name cannot be empty.");
        Value = value;
    }
}