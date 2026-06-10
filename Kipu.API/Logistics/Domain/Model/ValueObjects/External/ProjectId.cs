namespace Kipu.API.Logistics.Domain.Model.ValueObjects.External;

public sealed record ProjectId
{
    private const int MinValue = 0;

    public ProjectId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"ProjectId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }

    public int Value { get; }
    public override string ToString() => Value.ToString();
};