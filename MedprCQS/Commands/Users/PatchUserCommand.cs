using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Users;

public class PatchUserCommand : IRequest<int>
{
    public UserDTO User { get; set; }
    public List<PatchModel> PatchList { get; set; }
}