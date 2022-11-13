using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Vaccines;

public class PatchVaccineCommand: IRequest<int>
{
    public VaccineDTO Vaccine { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
