using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Prescriptions;

public class DeletePrescriptionCommand: IRequest<int>
{
    public PrescriptionDTO Prescription { get; set; }
}
