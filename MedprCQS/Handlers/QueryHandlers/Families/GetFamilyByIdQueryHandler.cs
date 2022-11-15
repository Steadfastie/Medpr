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

public class GetFamilyByIdQueryHandler: IRequestHandler<GetFamilyByIdQuery, FamilyDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamilyByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FamilyDTO> Handle(GetFamilyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Families
            .AsNoTracking()
            .FirstOrDefaultAsync(family => family.Id == request.Id, cancellationToken: cancellationToken);
        var family = _mapper.Map<FamilyDTO>(entity);

        return family;
    }
}
