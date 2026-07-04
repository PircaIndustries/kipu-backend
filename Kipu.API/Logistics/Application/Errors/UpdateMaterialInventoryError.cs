namespace Kipu.API.Logistics.Application.Errors;

public enum UpdateMaterialInventoryError
{
    MaterialInventoryNotFound,
    MaterialCatalogNotFound,
    DuplicatedMaterialInventory,
    UnexpectedError
}
