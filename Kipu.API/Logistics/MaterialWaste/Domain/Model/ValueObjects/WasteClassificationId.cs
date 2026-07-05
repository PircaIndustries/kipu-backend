namespace Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;

public record WasteClassificationId(string Value)
{
    public static WasteClassificationId Generate() => new($"wcls-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
}