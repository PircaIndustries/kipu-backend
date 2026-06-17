namespace Kipu.API.Budget.Interfaces.REST.Resources;

/// <summary>
/// Resource representing the required payload to create a new budget line item.
/// </summary>
public record CreateBudgetItemResource(int ProjectId, string ActivityName, string Details, decimal AssignedBudget);