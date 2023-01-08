using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Prescriptions;

public class PatchPrescriptionCommand : IRequest<int>
{
    public PrescriptionDTO Prescription { get; set; }
    public List<PatchModel> PatchList { get; set; }
}