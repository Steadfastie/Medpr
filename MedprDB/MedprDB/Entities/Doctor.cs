using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }

        public List<Prescription> Prescriptions { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}
