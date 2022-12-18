using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Appointments;

public class GetUpcomingPrescriptionsByUserIdQuery : IRequest<List<PrescriptionDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}
