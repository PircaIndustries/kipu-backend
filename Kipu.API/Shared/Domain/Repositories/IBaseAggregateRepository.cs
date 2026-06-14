namespace Kipu.API.Shared.Domain.Repositories;

public interface IBaseAggregateRepository<TEntity, TId> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
    
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
}