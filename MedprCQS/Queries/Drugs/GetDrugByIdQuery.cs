using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Drugs;

public class GetDrugByIdQuery: IRequest<DrugDTO>
{
    public Guid Id { get; set; }
}
