using Kipu.API.Logistics.MaterialWaste.Domain.Model.Queries;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;
using Kipu.API.Logistics.MaterialWaste.Domain.Repositories;

namespace Kipu.API.Logistics.MaterialWaste.Application.Services;

public class MaterialWasteQueryService(IMaterialWasteRepository wasteRepository) : IMaterialWasteQueryService
{
    public async Task<IEnumerable<Domain.Model.Aggregates.MaterialWaste>> Handle(GetAllMaterialWastesQuery query)
    {
        return await wasteRepository.ListAllAsync();
    }

    public async Task<Domain.Model.Aggregates.MaterialWaste?> Handle(GetMaterialWasteByIdQuery query)
    {
        return await wasteRepository.GetByIdAsync(new MaterialWasteId(query.Id));
    }
}