using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Vaccines;

public class GetVaccineByIdQuery : IRequest<VaccineDTO>
{
    public Guid Id { get; set; }
}