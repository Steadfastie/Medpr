using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB
{
    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime DateOfBirth { get; set; }


        public List<FamilyMember> FamilyMember { get; set; }

        public List<Prescription> Prescriptions { get; set; }

        public List<Vaccination> Vaccinations { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
