using AutoMapper;
using MediatR;
using MedprCQS.Commands.Prescriptions;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Prescriptions;

public class DeletePrescriptionCommandHandler : IRequestHandler<DeletePrescriptionCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeletePrescriptionCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(DeletePrescriptionCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Prescription>(request.Prescription);

        if (entity != null)
        {
            _context.Prescriptions.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Prescription));
        }
    }
}
