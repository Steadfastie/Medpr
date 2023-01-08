using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Vaccines;

public class DeleteVaccineCommand : IRequest<int>
{
    public VaccineDTO Vaccine { get; set; }
}