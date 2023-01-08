using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetOngoingPrescriptionsByUserIdQuery : IRequest<List<PrescriptionDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}