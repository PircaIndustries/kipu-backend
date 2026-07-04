using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;
using Kipu.API.Logistics.MachineryCatalog.Domain.Repositories;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Kipu.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace Kipu.API.Logistics.MachineryCatalog.Infrastructure.Persistence.EFC.Repositories;

public class MachineryCatalogRepository(AppDbContext context)
    : BaseAggregateRepository<Domain.Model.Aggregates.MachineryCatalog, string>(context), IMachineryCatalogRepository
{
}
