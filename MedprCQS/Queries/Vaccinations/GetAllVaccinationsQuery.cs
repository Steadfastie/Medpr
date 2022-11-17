using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Vaccinations;

public class GetAllVaccinationsQuery: IRequest<List<VaccinationDTO>>
{
}
