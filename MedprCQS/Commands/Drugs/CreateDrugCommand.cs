using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Drugs
{
    public class CreateDrugCommand : IRequest<int>
    {
        public DrugDTO Drug { get; set; }
    }
}