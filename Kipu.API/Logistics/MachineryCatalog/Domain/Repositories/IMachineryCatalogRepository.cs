using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Logistics.MachineryCatalog.Domain.Model.Aggregates;

namespace Kipu.API.Logistics.MachineryCatalog.Domain.Repositories;

public interface IMachineryCatalogRepository : IBaseAggregateRepository<Model.Aggregates.MachineryCatalog, string>
{
}
