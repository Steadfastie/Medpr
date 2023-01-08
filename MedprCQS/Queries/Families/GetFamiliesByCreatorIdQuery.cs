using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Families;

public class GetFamiliesByCreatorIdQuery : IRequest<List<FamilyDTO>>
{
    public Guid CreatorId { get; set; }
}