using Kipu.API.Progress.Domain.Model.ValueObjects;

namespace Kipu.API.Progress.Domain.Model.Aggregates;

public class ProgressItem
{
    public int Id { get; private set; }
    public int ProjectId { get; private set; }
    public TaskName TaskName { get; private set; } = null!;
    public decimal PlannedPercentage { get; private set; }
    public decimal ActualPercentage { get; private set; }

    // Required by Entity Framework Core
    protected ProgressItem() {}

    public ProgressItem(int projectId, TaskName taskName, decimal plannedPercentage)
    {
        ProjectId = projectId;
        TaskName = taskName;
        PlannedPercentage = plannedPercentage;
        ActualPercentage = 0;
    }

    public void UpdateActualProgress(decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 0 and 100.");
        ActualPercentage = percentage;
    }
}