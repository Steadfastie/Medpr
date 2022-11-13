using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Doctors;

public class PatchDoctorCommand: IRequest<int>
{
    public DoctorDTO Doctor { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
