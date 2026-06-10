namespace Kipu.API.Logistics.Domain.Model.ValueObjects;

public sealed record WarehouseLocation
{

    public WarehouseLocation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Warehouse location cannot be null, empty, or whitespace.", nameof(value));
        var parts = value.Split('-');
        if (parts.Length != 3)
            throw new ArgumentException("Location must follow the format 'Aisle-Rack-Shelf' (e.g., 'A-12-5').", nameof(value));

        var aisle = parts[0];
        var rack = parts[1];
        var shelf = parts[2];
        if (string.IsNullOrWhiteSpace(aisle))
            throw new ArgumentException("Aisle cannot be null or empty.", nameof(value));
            
        if (string.IsNullOrWhiteSpace(rack))
            throw new ArgumentException("Rack cannot be null or empty.", nameof(value));

        if (string.IsNullOrWhiteSpace(shelf))
            throw new ArgumentException("Shelf cannot be null or empty.", nameof(value));
        Value = value;
    }
    public string Value { get; }
    public override string ToString() => Value;
}