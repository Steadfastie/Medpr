using AutoMapper;
using MediatR;
using MedprCore.DTO;
using MedprCQS.Queries.Users;
using MedprDB;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.QueryHandlers.Users;

public class GetUserByNameQueryHandler : IRequestHandler<GetUserByLoginQuery, UserDTO?>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public GetUserByNameQueryHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDTO> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Login == request.Login, cancellationToken: cancellationToken);
        var drug = _mapper.Map<UserDTO>(entity);

        return drug;
    }
}