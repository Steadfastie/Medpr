using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Appointments;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Appointments;

public class GetUpcomingVaccinationsByUserIdQueryHandler : IRequestHandler<GetUpcomingVaccinationsByUserIdQuery, List<VaccinationDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetUpcomingVaccinationsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<VaccinationDTO>> Handle(GetUpcomingVaccinationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var vaccinations = await _context.Vaccinations
            .AsNoTracking()
            .Where(vaccination => vaccination.UserId == request.UserId && vaccination.Date >= request.Date)
            .Select(vaccination => _mapper.Map<VaccinationDTO>(vaccination))
            .ToListAsync(cancellationToken: cancellationToken);

        return vaccinations;
    }
}
