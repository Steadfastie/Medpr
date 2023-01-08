using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Drugs;

public class GetDrugByNameQuery : IRequest<DrugDTO>
{
    public string Name { get; set; }
}