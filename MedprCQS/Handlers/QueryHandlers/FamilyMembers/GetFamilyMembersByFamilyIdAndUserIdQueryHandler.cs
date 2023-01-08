using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.FamilyMembers;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.FamilyMembers;

public class GetFamilyMembersByFamilyIdAndUserIdQueryHandler : IRequestHandler<GetFamilyMembersByFamilyIdAndUserIdQuery, FamilyMemberDTO>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamilyMembersByFamilyIdAndUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FamilyMemberDTO> Handle(GetFamilyMembersByFamilyIdAndUserIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.FamilyMembers
            .AsNoTracking()
            .Where(member =>
                member.FamilyId.Equals(request.FamilyId) &&
                member.UserId.Equals(request.UserId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        var members = _mapper.Map<FamilyMemberDTO>(entities);

        return members;
    }
}