using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Users;

public class GetUserByIdQuery: IRequest<UserDTO>
{
    public Guid Id { get; set; }
}
