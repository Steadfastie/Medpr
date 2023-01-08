using MediatR;
using MedprCore.DTO;

namespace MedprCQS.Queries.FamilyMembers;

public class GetAllFamilyMembersQuery : IRequest<List<FamilyMemberDTO>>
{
}