using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Commands.Appointments;

public class DeleteAppointmentCommand : IRequest<int>
{
    public AppointmentDTO Appointment { get; set; }
}