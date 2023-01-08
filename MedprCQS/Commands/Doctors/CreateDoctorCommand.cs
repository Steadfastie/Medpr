using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Doctors
{
    public class CreateDoctorCommand : IRequest<int>
    {
        public DoctorDTO Doctor { get; set; }
    }
}