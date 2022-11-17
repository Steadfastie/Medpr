using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Vaccinations;

public class PatchVaccinationCommand: IRequest<int>
{
    public VaccinationDTO Vaccination { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
