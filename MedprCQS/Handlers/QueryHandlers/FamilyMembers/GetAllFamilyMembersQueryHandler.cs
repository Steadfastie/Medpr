using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.FamilyMembers;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.FamilyMembers;

public class GetAllFamilyMembersQueryHandler : IRequestHandler<GetAllFamilyMembersQuery, List<FamilyMemberDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllFamilyMembersQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyMemberDTO>> Handle(GetAllFamilyMembersQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.FamilyMembers
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<FamilyMemberDTO>>(entities);

        return drugs;
    }
}