using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Appointments
{
    public class CreateAppointmentCommand : IRequest<int>
    {
        public AppointmentDTO Appointment { get; set; }
    }
}