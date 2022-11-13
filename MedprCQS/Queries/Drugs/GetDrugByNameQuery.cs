using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Drugs;

public class GetDrugByNameQuery: IRequest<DrugDTO>
{
    public string Name { get; set; }
}
