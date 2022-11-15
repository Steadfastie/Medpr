using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Families;

public class PatchFamilyCommand: IRequest<int>
{
    public FamilyDTO Family { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
