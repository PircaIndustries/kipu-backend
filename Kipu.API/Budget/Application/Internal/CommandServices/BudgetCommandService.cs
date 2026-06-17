using Kipu.API.Budget.Application.Services;
using Kipu.API.Budget.Domain.Model.Aggregates;
using Kipu.API.Budget.Domain.Model.ValueObjects;
using Kipu.API.Budget.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Budget.Application.Internal.CommandServices;

/// <summary>
/// Internal application service implementing write operations for the Budget context.
/// </summary>
public class BudgetCommandService(
    IBudgetItemRepository budgetItemRepository,
    IUnitOfWork unitOfWork) : IBudgetCommandService
{
    /// <inheritdoc />
    public async Task<Result<BudgetItem, string>> HandleCreateBudgetItemAsync(int projectId, string activityName, string details, decimal assignedBudget)
    {
        try
        {
            var validatedName = new ActivityName(activityName);
            var budgetItem = new BudgetItem(projectId, validatedName, details, assignedBudget);

            await budgetItemRepository.AddAsync(budgetItem);
            await unitOfWork.CompleteAsync();

            return new Result<BudgetItem, string>.Success(budgetItem);
        }
        catch (ArgumentException ex)
        {
            return new Result<BudgetItem, string>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return new Result<BudgetItem, string>.Failure($"An error occurred while creating the budget item: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<BudgetItem, string>> HandleRegisterExpenseAsync(int budgetItemId, decimal amount, string description)
    {
        try
        {
            var budgetItem = await budgetItemRepository.FindByIdAsync(budgetItemId);
            if (budgetItem == null)
                return new Result<BudgetItem, string>.Failure($"Budget line item with ID {budgetItemId} was not found.");

            budgetItem.RegisterExpense(amount, description);

            budgetItemRepository.Update(budgetItem);
            await unitOfWork.CompleteAsync();

            return new Result<BudgetItem, string>.Success(budgetItem);
        }
        catch (InvalidOperationException ex)
        {
            return new Result<BudgetItem, string>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return new Result<BudgetItem, string>.Failure($"An error occurred while registering the expense: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<Result<BudgetItem, string>> HandleRequestExtensionAsync(int budgetItemId, decimal additionalAmount)
    {
        try
        {
            var budgetItem = await budgetItemRepository.FindByIdAsync(budgetItemId);
            if (budgetItem == null)
                return new Result<BudgetItem, string>.Failure($"Budget line item with ID {budgetItemId} was not found.");

            budgetItem.RequestExtension(additionalAmount);

            budgetItemRepository.Update(budgetItem);
            await unitOfWork.CompleteAsync();

            return new Result<BudgetItem, string>.Success(budgetItem);
        }
        catch (ArgumentException ex)
        {
            return new Result<BudgetItem, string>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return new Result<BudgetItem, string>.Failure($"An error occurred while processing the budget extension: {ex.Message}");
        }
    }
}