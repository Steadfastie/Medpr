using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Appointments;

public class DeleteAppointmentCommand: IRequest<int>
{
    public AppointmentDTO Appointment { get; set; }
}
