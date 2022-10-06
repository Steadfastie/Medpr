using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore.DTO;

public class PrescriptionDTO
{
    public Guid Id { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "DateTime2")]
    public DateTime EndDate { get; set; }

    public int Dose { get; set; }

    public Guid UserId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid DrugId { get; set; }
}
