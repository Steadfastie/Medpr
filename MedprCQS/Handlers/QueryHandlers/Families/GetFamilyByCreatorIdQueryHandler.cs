using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Families;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Families;

public class GetFamilyByCreatorIdQueryHandler: IRequestHandler<GetFamiliesByCreatorIdQuery, List<FamilyDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamilyByCreatorIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyDTO>> Handle(GetFamiliesByCreatorIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Families
            .AsNoTracking()
            .Where(family => family.Creator == request.CreatorId)
            .ToListAsync(cancellationToken: cancellationToken);
        var family = _mapper.Map<List<FamilyDTO>>(entities);

        return family;
    }
}
