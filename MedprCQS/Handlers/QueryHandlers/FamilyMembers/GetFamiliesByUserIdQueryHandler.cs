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

public class GetFamiliesByUserIdQueryHandler: IRequestHandler<GetFamiliesByUserIdQuery, List<FamilyDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamiliesByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyDTO>> Handle(GetFamiliesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.FamilyMembers
            .AsNoTracking()
            .Where(member => member.UserId.Equals(request.UserId))
            .Select(member => member.Family)
            .ToListAsync(cancellationToken: cancellationToken);

        var families = _mapper.Map<List<FamilyDTO>>(entities);

        return families;
    }
}
