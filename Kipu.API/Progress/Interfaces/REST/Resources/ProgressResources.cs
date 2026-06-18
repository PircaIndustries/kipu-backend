namespace Kipu.API.Progress.Interfaces.REST.Resources;

public record CreateProgressItemResource(int ProjectId, string TaskName, decimal PlannedPercentage);
public record UpdateProgressResource(decimal ActualPercentage);
public record ProgressItemResource(int Id, int ProjectId, string TaskName, decimal PlannedPercentage, decimal ActualPercentage);