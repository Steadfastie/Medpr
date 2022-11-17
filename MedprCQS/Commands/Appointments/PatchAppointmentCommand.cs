using MediatR;
using MedprCore;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Commands.Appointments;

public class PatchAppointmentCommand: IRequest<int>
{
    public AppointmentDTO Appointment { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
