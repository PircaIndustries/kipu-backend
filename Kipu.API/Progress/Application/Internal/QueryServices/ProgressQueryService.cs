using Kipu.API.Progress.Application.Services;
using Kipu.API.Progress.Domain.Model.Aggregates;
using Kipu.API.Progress.Domain.Repositories;

namespace Kipu.API.Progress.Application.Internal.QueryServices;

public class ProgressQueryService(IProgressItemRepository progressItemRepository) : IProgressQueryService
{
    public async Task<ProgressItem?> HandleGetByIdAsync(int id) => await progressItemRepository.FindByIdAsync(id);
    public async Task<IEnumerable<ProgressItem>> HandleListByProjectIdAsync(int projectId) => await progressItemRepository.FindByProjectIdAsync(projectId);
}