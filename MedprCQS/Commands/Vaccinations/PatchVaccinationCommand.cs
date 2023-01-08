using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Vaccinations;

public class PatchVaccinationCommand : IRequest<int>
{
    public VaccinationDTO Vaccination { get; set; }
    public List<PatchModel> PatchList { get; set; }
}