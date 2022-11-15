using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Families;

public class GetFamiliesByCreatorIdQuery: IRequest<List<FamilyDTO>>
{
    public Guid CreatorId { get; set; }
}
