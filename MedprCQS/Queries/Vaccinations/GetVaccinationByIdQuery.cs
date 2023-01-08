using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Vaccinations;

public class GetVaccinationByIdQuery : IRequest<VaccinationDTO>
{
    public Guid Id { get; set; }
}