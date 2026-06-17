using Kipu.API.Progress.Domain.Model.Aggregates;

namespace Kipu.API.Progress.Application.Services;

public interface IProgressQueryService
{
    Task<ProgressItem?> HandleGetByIdAsync(int id);
    Task<IEnumerable<ProgressItem>> HandleListByProjectIdAsync(int projectId);
}