using Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Queries;
using Kipu.API.Logistics.MaterialWaste.Domain.Repositories;

namespace Kipu.API.Logistics.MaterialWaste.Application.Services;

public class WasteClassificationQueryService(IWasteClassificationRepository classificationRepository) : IWasteClassificationQueryService
{
    public async Task<IEnumerable<WasteClassification>> Handle(GetAllWasteClassificationsQuery query)
    {
        return await classificationRepository.ListAllAsync();
    }
}