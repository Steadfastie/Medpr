using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.FamilyMembers
{
    public class CreateFamilyMemberCommand : IRequest<int>
    {
        public FamilyMemberDTO FamilyMember { get; set; }
    }
}