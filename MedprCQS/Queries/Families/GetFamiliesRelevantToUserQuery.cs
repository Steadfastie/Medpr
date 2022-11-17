using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Families;

public class GetFamiliesRelevantToUserQuery : IRequest<List<FamilyDTO>>
{
    public Guid UserId { get; set; }
}
