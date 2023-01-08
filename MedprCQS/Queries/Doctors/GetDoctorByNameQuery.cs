using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Doctors;

public class GetDoctorByNameQuery : IRequest<DoctorDTO>
{
    public string Name { get; set; }
}