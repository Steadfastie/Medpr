using MediatR;
using MedprCore;
using MedprCore.DTO;

namespace MedprCQS.Commands.Doctors;

public class PatchDoctorCommand : IRequest<int>
{
    public DoctorDTO Doctor { get; set; }
    public List<PatchModel> PatchList { get; set; }
}