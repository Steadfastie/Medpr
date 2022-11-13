using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Drugs
{
    public class DeleteDrugCommand: IRequest<int>
    {
        public DrugDTO Drug { get; set; }
    }
}
