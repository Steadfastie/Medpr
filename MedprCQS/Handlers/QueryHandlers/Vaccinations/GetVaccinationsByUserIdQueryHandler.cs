using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Vaccinations;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Vaccinations;

public class GetVaccinationsByUserIdQueryHandler : IRequestHandler<GetVaccinationsByUserIdQuery, List<VaccinationDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetVaccinationsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VaccinationDTO>> Handle(GetVaccinationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _context.Vaccinations
            .AsNoTracking()
            .Where(appointment => appointment.UserId == request.UserId)
            .Select(appointment => _mapper.Map<VaccinationDTO>(appointment))
            .ToListAsync();

        return appointments;
    }
}