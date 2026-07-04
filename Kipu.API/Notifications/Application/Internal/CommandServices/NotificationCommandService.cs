using Kipu.API.Notifications.Application.Services;
using Kipu.API.Notifications.Domain.Model.Aggregates;
using Kipu.API.Notifications.Domain.Model.Commands;
using Kipu.API.Notifications.Domain.Repositories;
using Kipu.API.Shared.Domain.Repositories;

namespace Kipu.API.Notifications.Application.Internal.CommandServices;

public class NotificationCommandService(
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : INotificationCommandService
{
    public async Task<Notification?> Handle(CreateNotificationCommand command)
    {
        var notification = new Notification(command.UserId, command.Message, command.Type, command.RelatedProjectId, command.Payload);
        await notificationRepository.AddAsync(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }

    public async Task<Notification?> Handle(AcceptNotificationCommand command)
    {
        var notification = await notificationRepository.FindByIdAsync(command.Id);
        if (notification == null) return null;
        notification.Status = "Accepted";
        notification.IsRead = true;
        notificationRepository.Update(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }

    public async Task<Notification?> Handle(RejectNotificationCommand command)
    {
        var notification = await notificationRepository.FindByIdAsync(command.Id);
        if (notification == null) return null;
        notification.Status = "Rejected";
        notification.IsRead = true;
        notificationRepository.Update(notification);
        await unitOfWork.CompleteAsync();
        return notification;
    }
}
