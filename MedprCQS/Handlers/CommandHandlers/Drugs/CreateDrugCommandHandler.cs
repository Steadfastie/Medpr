using AutoMapper;
using MediatR;
using MedprCQS.Commands.Drugs;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Drugs;

public class CreateDrugCommandHandler : IRequestHandler<CreateDrugCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateDrugCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateDrugCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Drug>(request.Drug);

        if (entity != null)
        {
            await _context.Drugs.AddAsync(entity, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Drug));
        }
    }
}
