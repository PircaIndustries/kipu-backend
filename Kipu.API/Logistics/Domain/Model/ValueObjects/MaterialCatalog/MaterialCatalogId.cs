namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public class MaterialCatalogId
{
    private const int MinValue = 0;
    public MaterialCatalogId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"MaterialId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }
    public int Value { get; }
    public override string ToString() => Value.ToString();
}