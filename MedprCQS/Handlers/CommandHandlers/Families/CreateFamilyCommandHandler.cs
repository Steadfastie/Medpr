using AutoMapper;
using MediatR;
using MedprCQS.Commands.Families;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Families;

public class CreateFamilyCommandHandler : IRequestHandler<CreateFamilyCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateFamilyCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateFamilyCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Family>(request.Family);

        if (entity != null)
        {
            await _context.Families.AddAsync(entity, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Family));
        }
    }
}
