using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Drugs;

public class GetAllDrugsQuery : IRequest<List<DrugDTO>>
{
}