using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Drugs;

public class DeleteDrugCommand : IRequest<int>
{
    public DrugDTO Drug { get; set; }
}