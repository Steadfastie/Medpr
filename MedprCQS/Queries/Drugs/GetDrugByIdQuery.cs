using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Drugs;

public class GetDrugByIdQuery : IRequest<DrugDTO>
{
    public Guid Id { get; set; }
}