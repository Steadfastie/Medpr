using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.FamilyMembers;

public class GetFamilyMembersByFamilyIdAndUserIdQuery: IRequest<FamilyMemberDTO>
{
    public Guid FamilyId { get; set; }
    public Guid UserId { get; set; }
}
