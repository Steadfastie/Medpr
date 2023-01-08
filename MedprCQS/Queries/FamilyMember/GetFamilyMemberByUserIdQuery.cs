using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.FamilyMembers;

public class GetFamilyMembersByUserIdQuery : IRequest<List<FamilyMemberDTO>>
{
    public Guid UserId { get; set; }
}