using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Families;

public class PatchFamilyCommand : IRequest<int>
{
    public FamilyDTO Family { get; set; }
    public List<PatchModel> PatchList { get; set; }
}