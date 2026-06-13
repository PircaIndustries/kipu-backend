namespace Kipu.API.Logistics.Application.Errors;

public enum UpdatePartialMaterialRequestError
{
    MaterialRequestNotFound,
    RequestAlreadyAccepted,
    RequestAlreadyRefused,
    UnexpectedError
}
