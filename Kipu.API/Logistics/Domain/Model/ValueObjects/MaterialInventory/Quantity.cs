namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record Quantity
{
  private const int MinValue = 0;
  public Quantity(int value)
  {
    if (value < MinValue) 
      throw new ArgumentException("Quantity cannot be negative");
        
    Value = value;
  }
  public int Value { get; }
  public override string ToString() => Value.ToString();
  public Quantity Add(Quantity amount) => new(Value + amount.Value);
  public Quantity Subtract(Quantity amount)
  {
    if (amount.Value > Value)
      throw new InvalidOperationException("Insufficient stock for the deduction.");
            
    return new Quantity(Value - amount.Value);
  }
};