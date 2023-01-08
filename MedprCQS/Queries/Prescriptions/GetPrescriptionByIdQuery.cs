using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Prescriptions;

public class GetPrescriptionByIdQuery : IRequest<PrescriptionDTO>
{
    public Guid Id { get; set; }
}