using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore.DTO;

public class AppointmentDTO
{
    public Guid Id { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime Date { get; set; }

    public string Place { get; set; }

    public Guid UserId { get; set; }

    public Guid DoctorId { get; set; }
}
