using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Queries;

public record GetAllMaterialRequestByRequestStatusQuery(RequestStatus RequestStatus);