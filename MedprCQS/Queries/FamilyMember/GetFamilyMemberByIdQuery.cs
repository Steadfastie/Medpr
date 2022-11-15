using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.FamilyMembers;

public class GetFamiliesByUserIdQuery: IRequest<List<FamilyDTO>>
{
    public Guid UserId { get; set; }
}
