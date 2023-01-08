using System.ComponentModel.DataAnnotations.Schema;

namespace MedprCore.DTO;

public class VaccinationDTO : INotifyUser
{
    public Guid Id { get; set; }
    public string? NotificationId { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime Date { get; set; }

    public int DaysOfProtection { get; set; }

    public Guid UserId { get; set; }

    public Guid VaccineId { get; set; }
}