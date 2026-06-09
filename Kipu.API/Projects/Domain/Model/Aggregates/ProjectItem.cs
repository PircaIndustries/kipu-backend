using Kipu.API.Shared.Domain.Model;
using System.Text.Json.Serialization;

namespace Kipu.API.Projects.Domain.Model.Aggregates;

public class ProjectItem : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int ProjectId { get; set; }
    
    [JsonIgnore]
    public Project? Project { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public ProjectItem() { }

    public ProjectItem(string name, DateTime startDate, DateTime endDate, int projectId)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        ProjectId = projectId;
    }
}
