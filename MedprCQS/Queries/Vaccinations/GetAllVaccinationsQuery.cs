using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Vaccinations;

public class GetAllVaccinationsQuery : IRequest<List<VaccinationDTO>>
{
}