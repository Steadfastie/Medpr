using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Users;

public class GetUserByLoginQuery : IRequest<UserDTO>
{
    public string Login { get; set; }
}