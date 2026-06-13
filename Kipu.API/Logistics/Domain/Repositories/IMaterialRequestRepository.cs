using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Model.ValueObjects;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.Domain.Repositories;

public interface IMaterialRequestRepository : IBaseRepository<MaterialRequest>
{
    Task<IEnumerable<MaterialRequest>> FindByRequestStatus(RequestStatus requestStatus, CancellationToken cancellationToken = default);
    Task<MaterialRequest?> FindByIdWithItemsAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaterialRequest>> ListWithItemsAsync(CancellationToken cancellationToken = default);
}
