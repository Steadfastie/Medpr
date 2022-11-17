using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Prescriptions;

public class CreatePrescriptionCommand: IRequest<int>
{
    public PrescriptionDTO Prescription { get; set; }
}
