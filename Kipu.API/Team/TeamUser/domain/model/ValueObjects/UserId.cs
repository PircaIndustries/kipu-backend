namespace Kipu.API.Team.TeamUser.domain.model.ValueObjects;

public record UserId(string Value)
{
    
    public UserId() : this($"us-{Guid.NewGuid().ToString("N")[..8]}") { }

    
}