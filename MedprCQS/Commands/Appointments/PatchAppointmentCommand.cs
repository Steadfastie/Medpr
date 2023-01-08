using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Appointments;

public class PatchAppointmentCommand : IRequest<int>
{
    public AppointmentDTO Appointment { get; set; }
    public List<PatchModel> PatchList { get; set; }
}