using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprDB.Entities
{
    public class Prescription : IBaseEntity
    {
        public Guid Id { get; set; }
        public string? NotificationId { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime EndDate { get; set; }

        public int Dose { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid DrugId { get; set; }
        public Drug Drug { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}