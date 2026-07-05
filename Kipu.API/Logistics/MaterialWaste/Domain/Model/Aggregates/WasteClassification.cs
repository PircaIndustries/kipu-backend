using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;

public class WasteClassification
{
    public WasteClassificationId Id { get; private set; }
    public string Name { get; private set; }
    
    private WasteClassification() {}

    public WasteClassification(WasteClassificationId id, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        Id = id;
        Name = name;
    }
}