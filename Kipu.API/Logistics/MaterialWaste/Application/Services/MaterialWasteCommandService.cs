using Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;
using Kipu.API.Logistics.MaterialWaste.Domain.Repositories;
using Kipu.API.Shared.Domain.Repositories;

// Asumiendo tu IUnitOfWork común

namespace Kipu.API.Logistics.MaterialWaste.Application.Services;

public class MaterialWasteCommandService(
    IMaterialWasteRepository wasteRepository, 
    IUnitOfWork unitOfWork) : IMaterialWasteCommandService
{
    private IMaterialWasteCommandService _materialWasteCommandServiceImplementation;

    public async Task<Domain.Model.Aggregates.MaterialWaste> Handle(CreateMaterialWasteCommand command)
    {
        var waste = new Domain.Model.Aggregates.MaterialWaste(
            MaterialWasteId.Generate(),
            command.ProjectId,
            command.MaterialId,
            command.Quantity,
            command.ClassificationType,
            command.Description,
            command.ReportedBy,
            command.PhotoUrl
        );

        await wasteRepository.AddAsync(waste);
        await unitOfWork.CompleteAsync();
        return waste;
    }

    public async Task<Domain.Model.Aggregates.MaterialWaste> Handle(UpdateMaterialWasteCommand command)
    {
        var waste = await wasteRepository.GetByIdAsync(new MaterialWasteId(command.Id))
                    ?? throw new KeyNotFoundException($"Material waste record {command.Id} not found.");

        waste.Update(command.Quantity, command.ClassificationType, command.Description);
        wasteRepository.Update(waste);
        await unitOfWork.CompleteAsync();
        return waste;
    }

    public async Task Handle(DeleteMaterialWasteCommand command)
    {
        var waste = await wasteRepository.GetByIdAsync(new MaterialWasteId(command.Id))
                    ?? throw new KeyNotFoundException($"Material waste record {command.Id} not found.");

        wasteRepository.Remove(waste);
        await unitOfWork.CompleteAsync();
    }
}