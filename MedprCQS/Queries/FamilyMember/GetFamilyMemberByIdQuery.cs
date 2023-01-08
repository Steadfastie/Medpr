using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.FamilyMembers;

public class GetFamilyMemberByIdQuery : IRequest<FamilyMemberDTO>
{
    public Guid Id { get; set; }
}