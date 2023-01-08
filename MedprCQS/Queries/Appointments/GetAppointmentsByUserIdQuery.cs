using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetAppointmentsByUserIdQuery : IRequest<List<AppointmentDTO>>
{
    public Guid UserId { get; set; }
}