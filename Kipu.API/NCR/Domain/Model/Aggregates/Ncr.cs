using Kipu.API.Ncr.Domain.Model.ValueObjects;
using Kipu.API.NCR.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.NCR.Domain.Model.Aggregates;

public class Ncr : IAuditableEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public NcrSeverity Severity { get; private set; }
    public int ProjectId { get; private set; }
    public string Speciality { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected Ncr()
    {
        Title = string.Empty;
        Description = string.Empty;
        Speciality = string.Empty;
    }

    public Ncr(string title, string description, NcrSeverity severity, int projectId, string speciality)
    {
        if (projectId <= 0)
            throw new ArgumentException("Invalid project identifier.");

        if (string.IsNullOrWhiteSpace(title)) 
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description)) 
            throw new ArgumentException("Description cannot be empty.");
        

        ProjectId = projectId;
        Title = title;
        Description = description;
        Severity = severity;
        Speciality = speciality;
    }
}