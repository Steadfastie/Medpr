using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Families;

public class GetFamilyByIdQuery : IRequest<FamilyDTO>
{
    public Guid Id { get; set; }
}