using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Vaccinations;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Vaccinations;

public class GetAllVaccinationsQueryHandler : IRequestHandler<GetAllVaccinationsQuery, List<VaccinationDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllVaccinationsQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VaccinationDTO>> Handle(GetAllVaccinationsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Vaccinations
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<VaccinationDTO>>(entities);

        return drugs;
    }
}