using Kipu.API.Budget.Domain.Model.Entities;
using Kipu.API.Budget.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Model;

namespace Kipu.API.Budget.Domain.Model.Aggregates;

/// <summary>
/// Aggregate root representing a project's budget line item (Partida Presupuestal).
/// </summary>
public class BudgetItem : IAuditableEntity
{
    public int Id { get; private set; }
    public int ProjectId { get; private set; }
    public ActivityName ActivityName { get; private set; }
    public string Details { get; private set; }
    public decimal AssignedBudget { get; private set; }
    public decimal ExecutedAmount { get; private set; }
    
    // Audit properties managed by the interceptor
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // Relationship: One BudgetItem has many BudgetTransactions
    private readonly List<BudgetTransaction> _transactions = new();
    public IReadOnlyCollection<BudgetTransaction> Transactions => _transactions.AsReadOnly();

    protected BudgetItem()
    {
        ActivityName = null!;
        Details = string.Empty;
    }

    public BudgetItem(int projectId, ActivityName activityName, string details, decimal assignedBudget)
    {
        if (projectId <= 0)
            throw new ArgumentException("Invalid project identifier.");

        if (assignedBudget < 0)
            throw new ArgumentException("Assigned budget cannot be negative.");

        ProjectId = projectId;
        ActivityName = activityName;
        Details = details ?? string.Empty;
        AssignedBudget = assignedBudget;
        ExecutedAmount = 0;
    }

    /// <summary>
    /// Registers a new expense transaction and updates the aggregate's executed amount accumulators.
    /// </summary>
    public void RegisterExpense(decimal amount, string description)
    {
        var available = AssignedBudget - ExecutedAmount;
        if (amount > available)
            throw new InvalidOperationException($"Insufficient funds. Available: {available}");

        var transaction = new BudgetTransaction(amount, description, Id);
        _transactions.Add(transaction);
        
        ExecutedAmount += amount;
    }

    /// <summary>
    /// Requests an extension to increase the assigned budget for this item.
    /// </summary>
    public void RequestExtension(decimal additionalAmount)
    {
        if (additionalAmount <= 0)
            throw new ArgumentException("Additional amount must be positive.");

        AssignedBudget += additionalAmount;
    }
}