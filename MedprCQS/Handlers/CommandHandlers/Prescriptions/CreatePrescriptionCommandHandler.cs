using AutoMapper;
using MediatR;
using MedprCQS.Commands.Prescriptions;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Prescriptions;

public class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreatePrescriptionCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreatePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Prescription>(request.Prescription);

        if (entity != null)
        {
            await _context.Prescriptions.AddAsync(entity, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Prescription));
        }
    }
}
