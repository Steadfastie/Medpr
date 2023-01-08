using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Families;

public class GetFamilyBySubstringQuery : IRequest<List<FamilyDTO>>
{
    public string Substring { get; set; }
}