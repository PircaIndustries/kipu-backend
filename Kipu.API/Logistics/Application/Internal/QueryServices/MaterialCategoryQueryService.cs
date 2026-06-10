using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries.MaterialCategory;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class MaterialCategoryQueryService(IMaterialCategoryRepository materialCategoryRepository)
    : IMaterialCategoryQueryService
{
    public async Task<MaterialCategory?> Handle(GetMaterialCategoryByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await materialCategoryRepository.FindByIdAsync(query.Id, cancellationToken);
    }
}