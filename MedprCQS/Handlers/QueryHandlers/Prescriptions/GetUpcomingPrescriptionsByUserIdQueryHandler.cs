using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Appointments;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Appointments;

public class GetUpcomingPrescriptionsByUserIdQueryHandler : IRequestHandler<GetUpcomingPrescriptionsByUserIdQuery, List<PrescriptionDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetUpcomingPrescriptionsByUserIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PrescriptionDTO>> Handle(GetUpcomingPrescriptionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var prescriptions = await _context.Prescriptions
            .AsNoTracking()
            .Where(prescription => prescription.UserId == request.UserId && prescription.Date >= request.Date)
            .Select(prescription => _mapper.Map<PrescriptionDTO>(prescription))
            .ToListAsync();

        return prescriptions;
    }
}