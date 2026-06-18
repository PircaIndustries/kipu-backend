namespace Kipu.API.Logistics.Application.Errors;

public enum UpdateMaterialCatalogError
{
    MaterialCatalogNotFound,
    CategoryNotFound,
    DuplicatedMaterialCatalog,
    UnexpectedError
}
