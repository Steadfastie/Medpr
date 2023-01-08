using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Doctors
{
    public class GetAllDoctorsQuery : IRequest<List<DoctorDTO>>
    {
    }
}