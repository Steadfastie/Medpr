using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Vaccines;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Vaccines;

public class GetVaccineByNameQueryHandler : IRequestHandler<GetVaccineByNameQuery, VaccineDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetVaccineByNameQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<VaccineDTO> Handle(GetVaccineByNameQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vaccines
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Name == request.Name, cancellationToken: cancellationToken);
        var drug = _mapper.Map<VaccineDTO>(entity);

        return drug;
    }
}