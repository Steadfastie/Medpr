using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Vaccines;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Vaccines;

public class GetVaccineByIdQueryHandler: IRequestHandler<GetVaccineByIdQuery, VaccineDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetVaccineByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VaccineDTO> Handle(GetVaccineByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vaccines
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<VaccineDTO>(entity);

        return drug;
    }
}
