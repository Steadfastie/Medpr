using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Users;

public class GetUserByLoginQuery: IRequest<UserDTO>
{
    public string Login { get; set; }
}
