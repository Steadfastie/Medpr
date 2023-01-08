using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetAppointmentByIdQuery : IRequest<AppointmentDTO>
{
    public Guid Id { get; set; }
}