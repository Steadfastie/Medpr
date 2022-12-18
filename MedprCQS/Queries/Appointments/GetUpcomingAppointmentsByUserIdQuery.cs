using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Appointments;

public class GetUpcomingAppointmentsByUserIdQuery : IRequest<List<AppointmentDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}
