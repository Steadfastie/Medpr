using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Vaccines;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Vaccines;

public class GetAllVaccinesQueryHandler : IRequestHandler<GetAllVaccinesQuery, List<VaccineDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllVaccinesQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VaccineDTO>> Handle(GetAllVaccinesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Vaccines
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<VaccineDTO>>(entities);

        return drugs;
    }
}