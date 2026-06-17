using Kipu.API.Progress.Domain.Model.Aggregates;
using Kipu.API.Shared.Application.Patterns;

namespace Kipu.API.Progress.Application.Services;

public interface IProgressCommandService
{
    Task<Result<ProgressItem, string>> HandleCreateProgressItemAsync(int projectId, string taskName, decimal plannedPercentage);
    Task<Result<ProgressItem, string>> HandleUpdateProgressAsync(int id, decimal actualPercentage);
}