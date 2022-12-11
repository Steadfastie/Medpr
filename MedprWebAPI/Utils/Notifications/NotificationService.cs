using Microsoft.AspNetCore.SignalR;

namespace MedprWebAPI.Utils.Notifications;

public class NotificationService: INotificationService
{
    private readonly IHubContext<EventNotificationHub, INotificationHub> _eventNotification;

    public NotificationService(IHubContext<EventNotificationHub, INotificationHub> eventNotificationHub)
    {
        _eventNotification = eventNotificationHub;
    }

    public async Task SendNotification(string message)
    {
        var notification = new Notification
        {
            Message = message
        };
        await _eventNotification.Clients.All.SendMessage(notification);
    }

}
