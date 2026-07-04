using Kipu.API.Notifications.Domain.Model.Aggregates;
using Kipu.API.Notifications.Domain.Model.Queries;
namespace Kipu.API.Notifications.Application.Services;
public interface INotificationQueryService { Task<IEnumerable<Notification>> Handle(GetNotificationsByUserIdQuery query); }
