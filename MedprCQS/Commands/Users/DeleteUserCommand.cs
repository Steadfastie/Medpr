using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Users;

public class DeleteUserCommand : IRequest<int>
{
    public UserDTO User { get; set; }
}