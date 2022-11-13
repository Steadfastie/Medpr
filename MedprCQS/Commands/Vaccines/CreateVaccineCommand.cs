using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Vaccines;

public class CreateVaccineCommand: IRequest<int>
{
    public VaccineDTO Vaccine { get; set; }
}
