using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Appointments;

public class GetUpcomingVaccinationsByUserIdQuery : IRequest<List<VaccinationDTO>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}
