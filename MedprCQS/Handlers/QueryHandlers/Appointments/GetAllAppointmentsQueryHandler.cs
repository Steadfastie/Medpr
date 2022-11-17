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

public class GetAllAppointmentsQueryHandler: IRequestHandler<GetAllAppointmentsQuery, List<AppointmentDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllAppointmentsQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AppointmentDTO>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Appointments
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<AppointmentDTO>>(entities);

        return drugs;
    }
}
