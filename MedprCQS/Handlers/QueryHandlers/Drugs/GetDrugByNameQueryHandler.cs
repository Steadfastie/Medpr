using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Drugs;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Drugs;

public class GetDrugByNameQueryHandler : IRequestHandler<GetDrugByNameQuery, DrugDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetDrugByNameQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DrugDTO> Handle(GetDrugByNameQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Drugs
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Name == request.Name, cancellationToken: cancellationToken);
        var drug = _mapper.Map<DrugDTO>(entity);

        return drug;
    }
}