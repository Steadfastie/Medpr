using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Prescriptions;

public class DeletePrescriptionCommand : IRequest<int>
{
    public PrescriptionDTO Prescription { get; set; }
}