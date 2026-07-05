using Kipu.API.Logistics.MaterialWaste.Domain.Model.Aggregates;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Commands;
using Kipu.API.Logistics.MaterialWaste.Domain.Model.Queries;

namespace Kipu.API.Logistics.MaterialWaste.Application.Services;

public interface IMaterialWasteCommandService
{
    Task<Domain.Model.Aggregates.MaterialWaste> Handle(CreateMaterialWasteCommand command);
    Task<Domain.Model.Aggregates.MaterialWaste> Handle(UpdateMaterialWasteCommand command);
    Task Handle(DeleteMaterialWasteCommand command);
}

public interface IMaterialWasteQueryService
{
    Task<IEnumerable<Domain.Model.Aggregates.MaterialWaste>> Handle(GetAllMaterialWastesQuery query);
    Task<Domain.Model.Aggregates.MaterialWaste?> Handle(GetMaterialWasteByIdQuery query);
}

public interface IWasteClassificationCommandService
{
    Task<WasteClassification> Handle(CreateWasteClassificationCommand command);
}

public interface IWasteClassificationQueryService
{
    Task<IEnumerable<WasteClassification>> Handle(GetAllWasteClassificationsQuery query);
}