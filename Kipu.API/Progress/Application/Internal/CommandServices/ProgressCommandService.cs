using Kipu.API.Progress.Application.Services;
using Kipu.API.Progress.Domain.Model.Aggregates;
using Kipu.API.Progress.Domain.Model.ValueObjects;
using Kipu.API.Progress.Domain.Repositories;
using Kipu.API.Shared.Application.Patterns;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Progress.Application.Internal.CommandServices;

public class ProgressCommandService(IProgressItemRepository progressItemRepository, IUnitOfWork unitOfWork) : IProgressCommandService
{
    public async Task<Result<ProgressItem, string>> HandleCreateProgressItemAsync(int projectId, string taskName, decimal plannedPercentage)
    {
        try
        {
            var name = new TaskName(taskName);
            var item = new ProgressItem(projectId, name, plannedPercentage);
            await progressItemRepository.AddAsync(item);
            await unitOfWork.CompleteAsync();
            return new Result<ProgressItem, string>.Success(item);
        }
        catch (Exception ex) { return new Result<ProgressItem, string>.Failure(ex.Message); }
    }

    public async Task<Result<ProgressItem, string>> HandleUpdateProgressAsync(int id, decimal actualPercentage)
    {
        try
        {
            var item = await progressItemRepository.FindByIdAsync(id);
            if (item == null) return new Result<ProgressItem, string>.Failure("Progress item not found.");
            item.UpdateActualProgress(actualPercentage);
            progressItemRepository.Update(item);
            await unitOfWork.CompleteAsync();
            return new Result<ProgressItem, string>.Success(item);
        }
        catch (Exception ex) { return new Result<ProgressItem, string>.Failure(ex.Message); }
    }
}