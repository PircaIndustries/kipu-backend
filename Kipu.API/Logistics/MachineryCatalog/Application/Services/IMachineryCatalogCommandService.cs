using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;

namespace Kipu.API.Logistics.MachineryCatalog.Application.Services;

public interface IMachineryCatalogCommandService
{
    Task<Domain.Model.Aggregates.MachineryCatalog?> Handle(CreateMachineryCatalogCommand command, CancellationToken cancellationToken = default);
    Task<Domain.Model.Aggregates.MachineryCatalog?> Handle(UpdateMachineryCatalogPartialCommand command, CancellationToken cancellationToken = default);
}
