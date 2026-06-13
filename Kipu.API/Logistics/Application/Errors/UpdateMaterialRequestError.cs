namespace Kipu.API.Logistics.Application.Errors;

public enum UpdateMaterialRequestError
{
    MaterialRequestNotFound,
    RequestAlreadyAccepted,
    RequestAlreadyRefused,
    UnexpectedError
}
