using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Users
{
    public class CreateUserCommand : IRequest<int>
    {
        public UserDTO User { get; set; }
    }
}