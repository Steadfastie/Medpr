using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.FamilyMembers;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.FamilyMembers;

public class GetFamilyMemberByIdQueryHandler : IRequestHandler<GetFamilyMemberByIdQuery, FamilyMemberDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetFamilyMemberByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FamilyMemberDTO> Handle(GetFamilyMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.FamilyMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<FamilyMemberDTO>(entity);

        return drug;
    }
}