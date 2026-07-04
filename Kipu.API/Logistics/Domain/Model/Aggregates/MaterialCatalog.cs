using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public partial class MaterialCatalog
{
    protected MaterialCatalog()
    {
        Name = null!;
        CategoryId = null!;
    }

    public MaterialCatalog(CreateMaterialCatalogCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Name = command.Name;
        CategoryId = command.CategoryId;
        MeasureUnit = command.MeasureUnit;
    }

    public void Update(UpdateMaterialCatalogCommand command)
    {
        Name = command.Name;
        CategoryId = command.CategoryId;
        MeasureUnit = command.MeasureUnit;
    }

    public int Id { get; private set; }
    public Name Name { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public MeasureUnit MeasureUnit { get; private set; }
    
}