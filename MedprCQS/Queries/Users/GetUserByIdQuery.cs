using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Users;

public class GetUserByIdQuery : IRequest<UserDTO>
{
    public Guid Id { get; set; }
}