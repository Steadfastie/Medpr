using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Families
{
    public class CreateFamilyCommand : IRequest<int>
    {
        public FamilyDTO Family { get; set; }
    }
}