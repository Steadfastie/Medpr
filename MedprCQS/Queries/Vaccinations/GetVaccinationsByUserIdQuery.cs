using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Vaccinations;

public class GetVaccinationsByUserIdQuery : IRequest<List<VaccinationDTO>>
{
    public Guid UserId { get; set; }
}