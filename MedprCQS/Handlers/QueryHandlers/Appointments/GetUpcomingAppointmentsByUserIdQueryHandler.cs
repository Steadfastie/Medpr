using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Appointments;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Appointments;

public class GetUpcomingAppointmentsByUserIdQueryHandler : IRequestHandler<GetUpcomingAppointmentsByUserIdQuery, List<AppointmentDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetUpcomingAppointmentsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AppointmentDTO>> Handle(GetUpcomingAppointmentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _context.Appointments
            .AsNoTracking()
            .Where(appointment => appointment.UserId == request.UserId && appointment.Date >= request.Date)
            .Select(appointment => _mapper.Map<AppointmentDTO>(appointment))
            .ToListAsync();

        return appointments;
    }
}
