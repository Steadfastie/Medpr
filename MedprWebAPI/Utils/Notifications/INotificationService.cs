namespace MedprCore.Abstractions;

public interface INotificationService
{
    Task SendNotification(string message, string type, string eventId, Guid userId);
}
