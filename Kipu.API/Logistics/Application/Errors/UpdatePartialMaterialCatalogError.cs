namespace Kipu.API.Logistics.Application.Errors;

public enum UpdatePartialMaterialCatalogError
{
    MaterialCatalogNotFound,
    CategoryNotFound,
    DuplicatedMaterialCatalog,
    UnexpectedError
}
