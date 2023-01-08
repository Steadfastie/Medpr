using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Users;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Users;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(drug => drug.Id == request.Id, cancellationToken: cancellationToken);
        var drug = _mapper.Map<UserDTO>(entity);

        return drug;
    }
}