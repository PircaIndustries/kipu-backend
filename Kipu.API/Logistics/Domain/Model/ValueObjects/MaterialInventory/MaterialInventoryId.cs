namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record MaterialInventoryId
{
    private const int MinValue = 0;
    public MaterialInventoryId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"InventoryId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }
    public int Value { get; }
    public override string ToString() => Value.ToString();
};