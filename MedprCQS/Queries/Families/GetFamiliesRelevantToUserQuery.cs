using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.Families;

public class GetFamiliesRelevantToUserQuery : IRequest<List<FamilyDTO>>
{
    public Guid UserId { get; set; }
}