using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Families;

public class GetAllFamiliesQuery : IRequest<List<FamilyDTO>>
{
}