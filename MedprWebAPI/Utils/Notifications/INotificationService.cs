namespace MedprWebAPI.Utils.Notifications;

public interface INotificationService
{
    Task SendNotification(string message);
}
