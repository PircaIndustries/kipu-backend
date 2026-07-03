using Kipu.API.Logistics.MachineryCatalog.Application.Services;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Queries;
using Kipu.API.Logistics.MachineryCatalog.Domain.Repositories;

namespace Kipu.API.Logistics.MachineryCatalog.Application.Internal.QueryServices;

public class MachineryCatalogQueryService(IMachineryCatalogRepository repository)
    : IMachineryCatalogQueryService
{
    public async Task<Domain.Model.Aggregates.MachineryCatalog?> Handle(GetMachineryCatalogByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Model.Aggregates.MachineryCatalog>> Handle(GetAllMachineryCatalogsQuery query, CancellationToken cancellationToken = default)
    {
        return await repository.ListAsync(cancellationToken);
    }
}
