using Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;

namespace Kipu.API.Logistics.MaterialWaste.Domain.Repositories;

public interface IWasteClassificationRepository
{
    Task AddAsync(WasteClassification wasteClassification);
    Task<IEnumerable<WasteClassification>> ListAllAsync();
}