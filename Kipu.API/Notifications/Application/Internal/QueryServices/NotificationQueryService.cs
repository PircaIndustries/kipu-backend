using Kipu.API.Notifications.Application.Services;
using Kipu.API.Notifications.Domain.Model.Aggregates;
using Kipu.API.Notifications.Domain.Model.Queries;
using Kipu.API.Notifications.Domain.Repositories;

namespace Kipu.API.Notifications.Application.Internal.QueryServices;

public class NotificationQueryService(INotificationRepository notificationRepository) : INotificationQueryService
{
    public async Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query)
    {
        return await notificationRepository.FindByUserIdAsync(query.UserId);
    }
}
