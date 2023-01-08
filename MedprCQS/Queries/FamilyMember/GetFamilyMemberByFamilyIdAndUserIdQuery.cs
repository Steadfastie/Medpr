using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.FamilyMembers;

public class GetFamilyMembersByFamilyIdAndUserIdQuery : IRequest<FamilyMemberDTO>
{
    public Guid FamilyId { get; set; }
    public Guid UserId { get; set; }
}