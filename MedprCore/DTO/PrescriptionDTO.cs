using System.ComponentModel.DataAnnotations.Schema;

namespace MedprCore.DTO;

public class PrescriptionDTO : INotifyUser
{
    public Guid Id { get; set; }
    public string? NotificationId { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime Date { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime EndDate { get; set; }

    public int Dose { get; set; }

    public Guid UserId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid DrugId { get; set; }
}