namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record RequestId
{
    private const int MinValue = 0;
    public RequestId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"RequestId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }
    public int Value { get; }
    public override string ToString() => Value.ToString();
};