using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Drugs;

public class PatchDrugCommand : IRequest<int>
{
    public DrugDTO Drug { get; set; }
    public List<PatchModel> PatchList { get; set; }
}