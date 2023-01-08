using System.ComponentModel.DataAnnotations.Schema;

namespace MedprCore.DTO;

public class AppointmentDTO : INotifyUser
{
    public Guid Id { get; set; }
    public string? NotificationId { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime Date { get; set; }

    public string Place { get; set; }

    public Guid UserId { get; set; }

    public Guid DoctorId { get; set; }
}