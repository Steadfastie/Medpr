using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprDB.Entities
{
    public class Vaccination : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? NotificationId { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        public int DaysOfProtection { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid VaccineId { get; set; }
        public Vaccine Vaccine { get; set; }
    }
}