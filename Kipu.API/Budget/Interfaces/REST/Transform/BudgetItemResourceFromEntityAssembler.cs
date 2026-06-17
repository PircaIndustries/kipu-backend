using Kipu.API.Budget.Domain.Model.Aggregates;
using Kipu.API.Budget.Interfaces.REST.Resources;

namespace Kipu.API.Budget.Interfaces.REST.Transform;

/// <summary>
/// Assembler tool to transform core BudgetItem aggregates into presentation-ready resources.
/// </summary>
public static class BudgetItemResourceFromEntityAssembler
{
    public static BudgetItemResource ToResourceFromEntity(BudgetItem entity)
    {
        return new BudgetItemResource(
            entity.Id,
            entity.ProjectId,
            entity.ActivityName.Value,
            entity.Details,
            entity.AssignedBudget,
            entity.ExecutedAmount
        );
    }
}