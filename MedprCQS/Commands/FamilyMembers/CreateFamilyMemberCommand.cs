using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.FamilyMembers
{
    public class CreateFamilyMemberCommand: IRequest<int>
    {
        public FamilyMemberDTO FamilyMember { get; set; }
    }
}
