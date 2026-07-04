using Kipu.API.Notifications.Domain.Model.Aggregates;
using Kipu.API.Notifications.Domain.Model.Commands;
namespace Kipu.API.Notifications.Application.Services;
public interface INotificationCommandService { Task<Notification?> Handle(CreateNotificationCommand command); Task<Notification?> Handle(AcceptNotificationCommand command); Task<Notification?> Handle(RejectNotificationCommand command); }
