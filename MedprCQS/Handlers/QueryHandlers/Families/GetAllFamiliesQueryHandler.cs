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

public class GetAllFamiliesQueryHandler: IRequestHandler<GetAllFamiliesQuery, List<FamilyDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllFamiliesQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyDTO>> Handle(GetAllFamiliesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Families
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<FamilyDTO>>(entities);

        return drugs;
    }
}
