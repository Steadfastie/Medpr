using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Drugs;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Drugs;

public class GetDrugByIdQueryHandler: IRequestHandler<GetDrugByIdQuery, DrugDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetDrugByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DrugDTO> Handle(GetDrugByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Drugs
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<DrugDTO>(entity);

        return drug;
    }
}
