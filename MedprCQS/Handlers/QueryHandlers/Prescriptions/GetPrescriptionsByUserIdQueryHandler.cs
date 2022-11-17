using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Prescriptions;
using MedprDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.QueryHandlers.Prescriptions;

public class GetPrescriptionsByUserIdQueryHandler : IRequestHandler<GetPrescriptionsByUserIdQuery, List<PrescriptionDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetPrescriptionsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PrescriptionDTO>> Handle(GetPrescriptionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _context.Prescriptions
            .AsNoTracking()
            .Where(appointment => appointment.UserId == request.UserId)
            .Select(appointment => _mapper.Map<PrescriptionDTO>(appointment))
            .ToListAsync();

        return appointments;
    }
}
