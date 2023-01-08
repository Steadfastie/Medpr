using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Doctors;

public class DeleteDoctorCommand : IRequest<int>
{
    public DoctorDTO Doctor { get; set; }
}