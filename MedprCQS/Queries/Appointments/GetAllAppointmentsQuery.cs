using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Appointments;

public class GetAllAppointmentsQuery : IRequest<List<AppointmentDTO>>
{
}