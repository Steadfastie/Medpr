using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Vaccinations;

public class DeleteVaccinationCommand : IRequest<int>
{
    public VaccinationDTO Vaccination { get; set; }
}