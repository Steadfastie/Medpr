using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Vaccines;

public class GetVaccineByNameQuery : IRequest<VaccineDTO>
{
    public string Name { get; set; }
}