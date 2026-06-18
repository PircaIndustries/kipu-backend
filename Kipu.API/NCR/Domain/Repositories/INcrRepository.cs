using Kipu.API.NCR.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.NCR.Domain.Repositories;

public interface INcrRepository : IBaseRepository<Model.Aggregates.Ncr>
{
    
}