using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Vaccines;

public class PatchVaccineCommand : IRequest<int>
{
    public VaccineDTO Vaccine { get; set; }
    public List<PatchModel> PatchList { get; set; }
}