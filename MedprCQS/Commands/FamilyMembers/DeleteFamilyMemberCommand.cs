using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.FamilyMembers;

public class DeleteFamilyMemberCommand : IRequest<int>
{
    public FamilyMemberDTO FamilyMember { get; set; }
}