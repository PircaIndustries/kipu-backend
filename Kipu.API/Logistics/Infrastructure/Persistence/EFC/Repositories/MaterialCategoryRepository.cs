using Kipu.API.Logistics.Domain.Model.Aggregates;
using Kipu.API.Logistics.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Kipu.API.Logistics.Infrastructure.Persistence.EFC.Repositories;

public class MaterialCategoryRepository(AppDbContext context)
    : BaseRepository<MaterialCategory>(context), IMaterialCategoryRepository
{
    
}