using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Prescriptions;

public class CreatePrescriptionCommand : IRequest<int>
{
    public PrescriptionDTO Prescription { get; set; }
}