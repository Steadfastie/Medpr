using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.FamilyMembers;

public class PatchFamilyMemberCommand : IRequest<int>
{
    public FamilyMemberDTO FamilyMember { get; set; }
    public List<PatchModel> PatchList { get; set; }
}