using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Queries;

namespace Kipu.API.Logistics.MachineryCatalog.Application.Services;

public interface IMachineryCatalogQueryService
{
    Task<Domain.Model.Aggregates.MachineryCatalog?> Handle(GetMachineryCatalogByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Model.Aggregates.MachineryCatalog>> Handle(GetAllMachineryCatalogsQuery query, CancellationToken cancellationToken = default);
}
