namespace Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;

public record MaterialWasteId(string Value)
{
    public static MaterialWasteId Generate() => new($"rmt-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
}