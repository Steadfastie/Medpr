using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Prescriptions;

public class GetPrescriptionsByUserIdQuery : IRequest<List<PrescriptionDTO>>
{
    public Guid UserId { get; set; }
}