using Kipu.API.Logistics.Application.Services;
using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.Queries;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Logistics.Domain.Repositories;

namespace Kipu.API.Logistics.Application.Internal.QueryServices;

public class MaterialRequestQueryService(IMaterialRequestRepository materialRequestRepository)
    : IMaterialRequestQueryService
{
    public async Task<MaterialRequest?> Handle(GetMaterialRequestByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRequestRepository.FindByIdWithItemsAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<MaterialRequest>> Handle(GetAllMaterialRequestQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRequestRepository.ListWithItemsAsync(cancellationToken);
    }

    public async Task<IEnumerable<MaterialRequest>> Handle(GetAllMaterialRequestByRequestStatusQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRequestRepository.FindByRequestStatus(query.RequestStatus, cancellationToken);
    }
}
