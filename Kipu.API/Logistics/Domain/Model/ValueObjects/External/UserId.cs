namespace Kipu.API.Logistics.Domain.Model.ValueObjects.External;

public record UserId
{
    private const int MinValue = 0;

    public UserId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"UserId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }

    public int Value { get; }
    public override string ToString() => Value.ToString();
};