using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries.MaterialCategory;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialCategoryQueryService
{
    Task<MaterialCategory?> Handle(GetMaterialCategoryByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialCategory?>> Handle(GetAllMaterialCategoriesQuery query, CancellationToken cancellationToken = default);
}