using System.Text.RegularExpressions;

namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record SocialReason
{
    private const int MinLength = 3;
    private const int MaxLength = 40;
    private const string Pattern = @"^[A-Za-z0-9ÑñáéíóúÁÉÍÓÚüÜ .,&\-]+$";

    public SocialReason(string value)
    {
        var trimmedValue = value?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedValue))
            throw new ArgumentException("Social reason cannot be null, empty, or whitespace.", nameof(value));

        if (trimmedValue.Length < MinLength)
            throw new ArgumentException($"Social reason is too short (minimum {MinLength} characters).", nameof(value));

        if (trimmedValue.Length > MaxLength)
            throw new ArgumentException($"Social reason length cannot exceed {MaxLength} characters according to Peruvian regulations.", nameof(value));

        if (!Regex.IsMatch(trimmedValue, Pattern))
        {
            throw new ArgumentException("Social reason contains invalid characters. Only alphanumeric, spaces, periods, commas, hyphens, and ampersands are allowed.", nameof(value));
        }

        Value = trimmedValue;
    }

    public string Value { get; }
    public override string ToString() => Value;
}