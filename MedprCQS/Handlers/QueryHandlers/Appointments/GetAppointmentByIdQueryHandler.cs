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

public class GetAppointmentByIdQueryHandler: IRequestHandler<GetAppointmentByIdQuery, AppointmentDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAppointmentByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentDTO> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<AppointmentDTO>(entity);

        return drug;
    }
}
