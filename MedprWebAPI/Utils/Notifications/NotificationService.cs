using MedprCore.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace MedprWebAPI.Utils.Notifications;

public class NotificationService: INotificationService
{
    private readonly IHubContext<EventNotificationHub, INotificationHub> _eventNotification;

    public NotificationService(IHubContext<EventNotificationHub, INotificationHub> eventNotificationHub)
    {
        _eventNotification = eventNotificationHub;
    }

    /// <summary>
    /// This method created and sends notification to Angular client
    /// </summary>
    /// <param name="message">Message to send in notification</param>
    /// <param name="type">Entity type</param>
    /// <param name="eventId">Id of entity</param>
    /// <returns></returns>
    public async Task SendNotification(string message, string type, string eventId)
    {
        var notification = new Notification
        {
            Message = message,
            Type = type,
            EventId = eventId
        };
        await _eventNotification.Clients.All.SendMessage(notification);
    }
}
