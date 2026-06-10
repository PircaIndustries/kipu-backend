using System.Text.RegularExpressions;

namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record Ruc
{
    private const int Length = 11;
    private const String Pattern = @"^\d{11}$";
    public Ruc(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("RUC cannot be null, empty, or whitespace.", nameof(value));

        if (value.Length != Length)
            throw new ArgumentException($"RUC Length cannot be different than {Length} characters.", nameof(value));
        if (!Regex.IsMatch(value, Pattern))
        {
            throw new ArgumentException("RUC only has to contained numeric characters", nameof(value));
        }

        if (!IsValidSunatRuc(value))
        {
            throw new ArgumentException("RUC is invalid according to SUNAT's algorithmic criteria.", nameof(value));
        }
        Value = value;
    }
    public string Value { get; }
    public override string ToString() => Value;

    private static bool IsValidSunatRuc(string ruc)
    {
        if (!ruc.StartsWith("10") && !ruc.StartsWith("15") && !ruc.StartsWith("17") && !ruc.StartsWith("20"))
            return false;
        int[] multipliers = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 10; i++)
        { 
            sum += (ruc[i] - '0') * multipliers[i];
        }
        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 10) checkDigit = 0;
        if (checkDigit == 11) checkDigit = 1;
        return checkDigit == (ruc[10] - '0');
    }
};