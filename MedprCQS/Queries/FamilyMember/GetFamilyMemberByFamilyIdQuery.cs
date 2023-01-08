using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.FamilyMembers;

public class GetFamilyMembersByFamilyIdQuery : IRequest<List<FamilyMemberDTO>>
{
    public Guid FamilyId { get; set; }
}