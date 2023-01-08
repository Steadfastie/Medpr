using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetUpcomingVaccinationsByUserIdQuery : IRequest<List<VaccinationDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}