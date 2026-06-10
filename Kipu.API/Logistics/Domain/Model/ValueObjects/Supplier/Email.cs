using System.Text.RegularExpressions;

namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record Email
{
    private const int MaxLength = 254;
    private const string Pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

    public Email(string value)
    {
        var trimmedValue = value?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedValue))
            throw new ArgumentException("Email cannot be null, empty, or whitespace.", nameof(value));
        if (trimmedValue.Length > MaxLength)
            throw new ArgumentException($"Email length cannot exceed {MaxLength} characters.", nameof(value));
        if (!Regex.IsMatch(trimmedValue, Pattern))
        {
            throw new ArgumentException("Email format is invalid.", nameof(value));
        }
        Value = trimmedValue.ToLowerInvariant();
    }

    public string Value { get; }
    public override string ToString() => Value;
}