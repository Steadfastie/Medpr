using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Vaccinations;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Vaccinations;

public class GetVaccinationByIdQueryHandler: IRequestHandler<GetVaccinationByIdQuery, VaccinationDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetVaccinationByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VaccinationDTO> Handle(GetVaccinationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vaccinations
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<VaccinationDTO>(entity);

        return drug;
    }
}
