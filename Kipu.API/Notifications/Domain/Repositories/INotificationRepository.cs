using Kipu.API.Notifications.Domain.Model.Aggregates;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Notifications.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IEnumerable<Notification>> FindByUserIdAsync(string userId);
}
