using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.FamilyMembers;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.FamilyMembers;

public class GetFamilyMembersByFamilyIdQueryHandler: IRequestHandler<GetFamilyMembersByFamilyIdQuery, List<FamilyMemberDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamilyMembersByFamilyIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyMemberDTO>> Handle(GetFamilyMembersByFamilyIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.FamilyMembers
            .AsNoTracking()
            .Where(member => member.FamilyId.Equals(request.FamilyId))
            .ToListAsync(cancellationToken: cancellationToken);

        var members = _mapper.Map<List<FamilyMemberDTO>>(entities);

        return members;
    }
}
