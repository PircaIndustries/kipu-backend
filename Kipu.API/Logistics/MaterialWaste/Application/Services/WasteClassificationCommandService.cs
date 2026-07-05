using Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.ValueObjects;
using Kipu.API.Logistics.MaterialWaste.Domain.Repositories;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Logistics.MaterialWaste.Application.Services;

public class WasteClassificationCommandService(
    IWasteClassificationRepository classificationRepository,
    IUnitOfWork unitOfWork) : IWasteClassificationCommandService
{
    private IWasteClassificationCommandService _wasteClassificationCommandServiceImplementation;

    public async Task<WasteClassification> Handle(CreateWasteClassificationCommand command)
    {
        var classification = new WasteClassification(
            WasteClassificationId.Generate(),
            command.Name
        );

        await classificationRepository.AddAsync(classification);
        await unitOfWork.CompleteAsync();
        return classification;
    }
}