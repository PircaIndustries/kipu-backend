namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record CategoryId
{
    private const int MinValue = 0;
    public CategoryId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"CategoryId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }
    public int Value { get; }
    public override string ToString() => Value.ToString();
}