using Kipu.API.Logistics.MachineryCatalog.Application.Services;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Commands;
using Kipu.API.Logistics.MachineryCatalog.Domain.Repositories;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.MachineryCatalog.Application.Internal.CommandServices;

public class MachineryCatalogCommandService(
    IMachineryCatalogRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<MachineryCatalogCommandService> logger)
    : IMachineryCatalogCommandService
{
    public async Task<Domain.Model.Aggregates.MachineryCatalog?> Handle(CreateMachineryCatalogCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var catalog = new Domain.Model.Aggregates.MachineryCatalog(command);
            await repository.AddAsync(catalog, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return catalog;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating machinery catalog");
            return null;
        }
    }

    public async Task<Domain.Model.Aggregates.MachineryCatalog?> Handle(UpdateMachineryCatalogPartialCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var catalog = await repository.FindByIdAsync(command.Id, cancellationToken);
            if (catalog is null)
            {
                logger.LogWarning("Machinery catalog with ID {Id} not found", command.Id);
                return null;
            }
            catalog.UpdatePartial(command);
            await unitOfWork.CompleteAsync(cancellationToken);
            return catalog;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating machinery catalog {Id}", command.Id);
            return null;
        }
    }
}
