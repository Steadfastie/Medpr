using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Drugs;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Drugs;

public class GetAllDrugsQueryHandler : IRequestHandler<GetAllDrugsQuery, List<DrugDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetAllDrugsQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DrugDTO>> Handle(GetAllDrugsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Drugs
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
        var drugs = _mapper.Map<List<DrugDTO>>(entities);

        return drugs;
    }
}