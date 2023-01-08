using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Prescriptions;

public class GetAllPrescriptionsQuery : IRequest<List<PrescriptionDTO>>
{
}