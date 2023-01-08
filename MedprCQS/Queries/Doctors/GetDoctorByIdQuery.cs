using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Doctors;

public class GetDoctorByIdQuery : IRequest<DoctorDTO>
{
    public Guid Id { get; set; }
}