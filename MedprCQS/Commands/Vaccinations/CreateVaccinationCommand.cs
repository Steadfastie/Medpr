using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Vaccinations;

public class CreateVaccinationCommand : IRequest<int>
{
    public VaccinationDTO Vaccination { get; set; }
}