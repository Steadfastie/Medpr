using MediatR;
using MedprCore.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Queries.Doctors;

public class GetDoctorByNameQuery: IRequest<DoctorDTO>
{
    public string Name { get; set; }
}
