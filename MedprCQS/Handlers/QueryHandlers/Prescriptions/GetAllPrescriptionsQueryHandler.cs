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

public class GetAllPrescriptionsQueryHandler: IRequestHandler<GetAllPrescriptionsQuery, List<PrescriptionDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllPrescriptionsQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PrescriptionDTO>> Handle(GetAllPrescriptionsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Prescriptions
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<PrescriptionDTO>>(entities);

        return drugs;
    }
}
