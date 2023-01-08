using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetUpcomingPrescriptionsByUserIdQuery : IRequest<List<PrescriptionDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}