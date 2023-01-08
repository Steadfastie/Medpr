using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Appointments;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Appointments;

public class GetAppointmentsByUserIdQueryHandler : IRequestHandler<GetAppointmentsByUserIdQuery, List<AppointmentDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAppointmentsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AppointmentDTO>> Handle(GetAppointmentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _context.Appointments
            .AsNoTracking()
            .Where(appointment => appointment.UserId == request.UserId)
            .Select(appointment => _mapper.Map<AppointmentDTO>(appointment))
            .ToListAsync();

        return appointments;
    }
}