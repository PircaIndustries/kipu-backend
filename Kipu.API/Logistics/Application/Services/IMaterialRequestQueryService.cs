using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Application.Services;

public interface IMaterialRequestQueryService
{
    Task<MaterialRequest?> Handle(GetMaterialRequestByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialRequest>> Handle(GetAllMaterialRequestQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialRequest>> Handle(GetAllMaterialRequestByRequestStatusQuery query, CancellationToken cancellationToken = default);
}
