using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Vaccines;

public class CreateVaccineCommand : IRequest<int>
{
    public VaccineDTO Vaccine { get; set; }
}