using Kipu.API.Logistics.Domain.Model.Commands;
using Kipu.API.Logistics.Domain.Model.ValueObjects;

namespace Kipu.API.Logistics.Domain.Model.Aggregates;

public partial class MaterialCategory
{
    protected MaterialCategory()
    {
        Name = null!;
        Description = string.Empty;
    }

    public MaterialCategory(CreateMaterialCategoryCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Name = command.Name;
        Description = command.Description ?? string.Empty;
        IsActive = command.IsActive;
    }
    public void Update(UpdateMaterialCategoryCommand command)
    {
        Name = command.Name;
        Description = command.Description;
        IsActive = command.IsActive;
    }
    public int Id { get; private set; }
    public Name Name { get; private set; }
    public String Description { get; private set; }
    public Boolean IsActive { get; private set; }
}