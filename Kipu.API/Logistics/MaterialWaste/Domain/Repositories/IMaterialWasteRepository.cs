using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.MaterialWaste.Domain.Repositories;

public interface IMaterialWasteRepository
{
    Task AddAsync(Model.Aggregates.MaterialWaste materialWaste);
    Task<Model.Aggregates.MaterialWaste?> GetByIdAsync(MaterialWasteId id);
    Task<IEnumerable<Model.Aggregates.MaterialWaste>> ListAllAsync();
    void Update(Model.Aggregates.MaterialWaste materialWaste);
    void Remove(Model.Aggregates.MaterialWaste materialWaste);
}