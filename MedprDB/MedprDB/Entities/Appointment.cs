using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprDB.Entities
{
    public class Appointment : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? NotificationId { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        public string Place { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}