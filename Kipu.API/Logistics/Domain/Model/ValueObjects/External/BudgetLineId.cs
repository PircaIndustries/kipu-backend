namespace Kipu.API.Logistics.Domain.Model.ValueObjects.External;

public sealed record BudgetLineId
{
    private const int MinValue = 0;

    public BudgetLineId(int value)
    {
        if (value <= MinValue)
        {
            throw new ArgumentException($"BudgetLineId cannot be less than {MinValue}.", nameof(value));
        }

        Value = value;
    }

    public int Value { get; }
    public override string ToString() => Value.ToString();
}