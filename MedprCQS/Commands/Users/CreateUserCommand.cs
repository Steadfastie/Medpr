using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Users
{
    public class CreateUserCommand: IRequest<int>
    {
        public UserDTO User { get; set; }
    }
}
