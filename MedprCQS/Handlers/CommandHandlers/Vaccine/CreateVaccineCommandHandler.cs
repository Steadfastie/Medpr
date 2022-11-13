using AutoMapper;
using MediatR;
using MedprCQS.Commands.Vaccines;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Vaccines;

public class CreateVaccineCommandHandler : IRequestHandler<CreateVaccineCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateVaccineCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateVaccineCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vaccine>(request.Vaccine);

        if (entity != null)
        {
            await _context.Vaccines.AddAsync(entity, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Vaccine));
        }
    }
}
