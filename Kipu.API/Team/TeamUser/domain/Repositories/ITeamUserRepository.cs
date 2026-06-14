using Kipu.API.Shared.Domain.Repositories;
using Kipu.API.Team.TeamUser.domain.model.ValueObjects;

namespace Kipu.API.Team.TeamUser.domain.Repositories;

/// <summary>
/// Interfaz del repositorio para el agregado TeamUser.
/// Hereda de IBaseRepository para obtener métodos genéricos de CRUD.
/// </summary>
public interface ITeamUserRepository : IBaseAggregateRepository<model.Aggregates.TeamUser, UserId>
{
        Task<IEnumerable<model.Aggregates.TeamUser>> FindByIsActive(string projectId, bool isActive);
        
        Task<IEnumerable<model.Aggregates.TeamUser>> FindByRole(string projectId, string role);
        
        Task<IEnumerable<model.Aggregates.TeamUser>> FindByFilter(string projectId, string? globalSearch, string? role, bool? isActive);
}