using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB
{
    public class Appointment
    {
        public Guid Id { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        public string Place { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
