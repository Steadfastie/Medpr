namespace MedprCore.DTO;

public interface INotifyUser
{
    public string? NotificationId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}
