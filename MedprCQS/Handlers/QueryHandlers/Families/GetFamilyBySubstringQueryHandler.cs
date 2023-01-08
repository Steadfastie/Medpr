using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Families;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Families;

public class GetFamilyBySubstringQueryHandler : IRequestHandler<GetFamilyBySubstringQuery, List<FamilyDTO>>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamilyBySubstringQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<FamilyDTO>> Handle(GetFamilyBySubstringQuery request, CancellationToken cancellationToken)
    {
        var entities = _context.Families
            .AsNoTracking()
            .AsEnumerable()
            .Where(family => family.Surname.IndexOf(request.Substring, StringComparison.InvariantCultureIgnoreCase) != -1)
            .ToList();

        var family = _mapper.Map<List<FamilyDTO>>(entities);

        return family;
    }
}