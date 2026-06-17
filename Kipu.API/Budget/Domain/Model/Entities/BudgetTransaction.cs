namespace Kipu.API.Budget.Domain.Model.Entities;

/// <summary>
/// Entity representing an expense transaction linked to a specific budget item.
/// </summary>
public class BudgetTransaction
{
    public int Id { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public int BudgetItemId { get; private set; }

    protected BudgetTransaction()
    {
        Description = string.Empty;
    }

    public BudgetTransaction(decimal amount, string description, int budgetItemId)
    {
        if (amount <= 0)
            throw new ArgumentException("Transaction amount must be greater than zero.");

        Amount = amount;
        Description = string.IsNullOrWhiteSpace(description) ? "Registered Expense" : description.Trim();
        Date = DateTime.UtcNow;
        BudgetItemId = budgetItemId;
    }
}