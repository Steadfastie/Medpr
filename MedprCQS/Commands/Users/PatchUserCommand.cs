using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Users;

public class PatchUserCommand: IRequest<int>
{
    public UserDTO User { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
