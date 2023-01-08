using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetUpcomingAppointmentsByUserIdQuery : IRequest<List<AppointmentDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}