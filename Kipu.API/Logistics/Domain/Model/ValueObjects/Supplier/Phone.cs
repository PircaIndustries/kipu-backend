using System.Text.RegularExpressions;

namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record Phone
{
    private const int MinLength = 7;
    private const int MaxLength = 15;
    private const string Pattern = @"^\+?[0-9]{7,15}$";
    public Phone(string value)
    {
        var cleanedValue = value?.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Trim();

        if (string.IsNullOrWhiteSpace(cleanedValue))
            throw new ArgumentException("Phone number cannot be null, empty, or whitespace.", nameof(value));

        if (cleanedValue.Length < MinLength)
            throw new ArgumentException($"Phone number is too short (minimum {MinLength} characters).", nameof(value));

        if (cleanedValue.Length > MaxLength)
            throw new ArgumentException($"Phone number length cannot exceed {MaxLength} characters.", nameof(value));

        if (!Regex.IsMatch(cleanedValue, Pattern))
        {
            throw new ArgumentException("Phone number format is invalid. Only digits and an optional leading '+' are allowed.", nameof(value));
        }

        Value = cleanedValue;
    }
    public string Value { get; }
    public override string ToString() => Value;
}