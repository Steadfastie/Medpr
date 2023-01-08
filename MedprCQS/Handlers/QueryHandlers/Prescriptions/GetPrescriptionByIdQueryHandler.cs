using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Prescriptions;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Prescriptions;

public class GetPrescriptionByIdQueryHandler : IRequestHandler<GetPrescriptionByIdQuery, PrescriptionDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetPrescriptionByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PrescriptionDTO> Handle(GetPrescriptionByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Prescriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<PrescriptionDTO>(entity);

        return drug;
    }
}