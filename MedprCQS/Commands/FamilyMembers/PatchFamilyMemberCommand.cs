using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.FamilyMembers;

public class PatchFamilyMemberCommand: IRequest<int>
{
    public FamilyMemberDTO FamilyMember { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
