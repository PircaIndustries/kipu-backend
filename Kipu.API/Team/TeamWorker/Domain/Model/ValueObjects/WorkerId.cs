namespace Kipu.API.Team.TeamWorker.Domain.Model.ValueObjects;

public record WorkerId(string Value)
{
    public WorkerId() : this($"wrk-{Guid.NewGuid().ToString("N")}") { }
}