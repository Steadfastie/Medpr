using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprDB.Entities
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string? FullName { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? DateOfBirth { get; set; }

        public List<FamilyMember> FamilyMember { get; set; }

        public List<Prescription> Prescriptions { get; set; }

        public List<Vaccination> Vaccinations { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}