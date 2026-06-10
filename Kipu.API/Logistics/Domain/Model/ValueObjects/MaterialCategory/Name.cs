using System.Text.RegularExpressions;

namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record Name
{
    private const int MaxLength = 100;
    private const string Pattern = @"^[a-zA-ZáéíóúñÑüÜ\s\-_0-9]+$";
    private const string ErrorPattern = @"^[0-9]+$";
    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(value));
        
        if (value.Length > MaxLength)
            throw new ArgumentException($"Name Length cannot be greater than {MaxLength} characters.", nameof(value));
        if (Regex.IsMatch(value, ErrorPattern))
        {
            throw new ArgumentException("Name cannot contain only numeric characters. It must include letters.", nameof(value));
        }
        if (!Regex.IsMatch(value, Pattern))
        {
            throw new ArgumentException("Name must contain only letters, numbers, spaces, hyphens or underscores.", nameof(value));
        }
        
        Value = value;
    }
    public string Value { get; }
    public override string ToString() => Value;

};