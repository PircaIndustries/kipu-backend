namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record SupplierId
{
    private const int MinValue = 0;
    public SupplierId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"SupplierId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }
    public int Value { get; }
    public override string ToString() => Value.ToString();
};