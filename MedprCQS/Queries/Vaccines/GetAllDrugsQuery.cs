using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Vaccines
{
    public class GetAllVaccinesQuery : IRequest<List<VaccineDTO>>
    {
    }
}