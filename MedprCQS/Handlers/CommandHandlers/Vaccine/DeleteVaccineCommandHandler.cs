using AutoMapper;
using MediatR;
using MedprCQS.Commands.Vaccines;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Vaccines;

public class DeleteVaccineCommandHandler : IRequestHandler<DeleteVaccineCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteVaccineCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(DeleteVaccineCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vaccine>(request.Vaccine);

        if (entity != null)
        {
            _context.Vaccines.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Vaccine));
        }
    }
}
