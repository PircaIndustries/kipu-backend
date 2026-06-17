namespace Kipu.API.Budget.Interfaces.REST.Resources;

/// <summary>
/// Resource representing the data outbound layout for a budget line item.
/// </summary>
public record BudgetItemResource(int Id, int ProjectId, string ActivityName, string Details, decimal AssignedBudget, decimal ExecutedAmount);