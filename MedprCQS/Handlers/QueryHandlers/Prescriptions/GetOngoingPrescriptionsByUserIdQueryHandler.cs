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

public class GetOngoingPrescriptionsByUserIdQueryHandler : IRequestHandler<GetOngoingPrescriptionsByUserIdQuery, List<PrescriptionDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetOngoingPrescriptionsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PrescriptionDTO>> Handle(GetOngoingPrescriptionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var prescriptions = await _context.Prescriptions
            .AsNoTracking()
            .Where(prescription => 
                prescription.UserId == request.UserId && 
                prescription.Date <= request.Date && 
                prescription.EndDate >= request.Date)
            .Select(prescription => _mapper.Map<PrescriptionDTO>(prescription))
            .ToListAsync();

        return prescriptions;
    }
}
