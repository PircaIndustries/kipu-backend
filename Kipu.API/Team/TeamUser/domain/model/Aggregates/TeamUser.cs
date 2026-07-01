using Kipu.API.Team.TeamUser.domain.model.ValueObjects;

namespace Kipu.API.Team.TeamUser.domain.model.Aggregates;

public partial class TeamUser
{
    public UserId Id { get; set; }
    public int? UserId { get; private set; }
    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public bool IsActive { get; private set; }
    public string Role { get; private set; }
    public string ProjectId { get; private set; }

    protected TeamUser()
    {
        FullName = string.Empty;
        Role = string.Empty;
        ProjectId = string.Empty;
    }

    public TeamUser(int userId, string fullName, Email email, string role, string projectId)
    {
        Id = new UserId();
        UserId = userId;
        FullName = fullName;
        Email = email;
        IsActive = true;
        Role = role;
        ProjectId = projectId;
    }

    public TeamUser(UserId id, int? userId, string fullName, Email email, string role, string projectId)
    {
        Id = id;
        UserId = userId;
        FullName = fullName;
        Email = email;
        IsActive = true;
        Role = role;
        ProjectId = projectId;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
