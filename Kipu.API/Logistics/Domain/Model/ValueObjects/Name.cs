using System.Text.RegularExpressions;

namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record Name
{
    private const int MaxLength = 255;
    private const string Pattern = @"^[a-zA-Z0-9]+$";
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"Name Length cannot be greater than {MaxLength} characters.", nameof(value));
        if (Regex.IsMatch(value, Pattern))
        {
            throw new ArgumentException("Name only has to contained numeric characters", nameof(value));
        }
    }
};