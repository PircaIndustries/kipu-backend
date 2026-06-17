using Kipu.API.Ncr.Domain.Model.ValueObjects;
using Kipu.API.NCR.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.NCR.Domain.Model.Aggregates;

public class Ncr : IAuditableEntity
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string TaskName { get; private set; }
    public NcrSeverity Severity { get; private set; }
    public int ProjectId { get; private set; }
    public NcrStatus Status { get; private set; }
    public string RootCause { get; private set; }
    public string CorrectiveAction { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected Ncr()
    {
        Title = string.Empty;
        Description = string.Empty;
        TaskName = string.Empty;
        RootCause = string.Empty;
        CorrectiveAction = string.Empty;
    }

    public Ncr(string title, string description, string taskName, NcrSeverity severity, int projectId)
    {
        if (projectId <= 0)
            throw new ArgumentException("Invalid project identifier.");

        if (string.IsNullOrWhiteSpace(title)) 
            throw new ArgumentException("Title cannot be empty.");

        if (string.IsNullOrWhiteSpace(description)) 
            throw new ArgumentException("Description cannot be empty.");

        if (string.IsNullOrWhiteSpace(taskName)) 
            throw new ArgumentException("Task name cannot be empty.");

        ProjectId = projectId;
        Title = title;
        Description = description;
        TaskName = taskName;
        Severity = severity;
        Status = NcrStatus.Opened;
        RootCause = string.Empty;
        CorrectiveAction = string.Empty;
    }

    public void Resolve(string rootCause, string correctiveAction)
    {
        if (Status == NcrStatus.Closed)
            throw new InvalidOperationException("Cannot resolve a closed NCR.");

        if (string.IsNullOrWhiteSpace(rootCause))
            throw new ArgumentException("Root cause analysis is required to resolve.");

        if (string.IsNullOrWhiteSpace(correctiveAction))
            throw new ArgumentException("Corrective action plan is required to resolve.");

        RootCause = rootCause;
        CorrectiveAction = correctiveAction;
        Status = NcrStatus.Resolved;
    }

    public void Close()
    {
        if (Status != NcrStatus.Resolved)
            throw new InvalidOperationException("NCR must be resolved before closing.");

        Status = NcrStatus.Closed;
    }
}