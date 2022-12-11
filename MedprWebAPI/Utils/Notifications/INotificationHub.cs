namespace MedprWebAPI.Utils.Notifications;

public interface INotificationHub
{
    public Task SendMessage(Notification notification);
}
