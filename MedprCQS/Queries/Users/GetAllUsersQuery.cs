using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Users;

public class GetAllUsersQuery : IRequest<List<UserDTO>>
{
}