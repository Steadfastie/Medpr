using AutoMapper;
using MediatR;
using MedprCQS.Commands.Families;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Families;

public class DeleteFamilyCommandHandler : IRequestHandler<DeleteFamilyCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteFamilyCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(DeleteFamilyCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Family>(request.Family);

        if (entity != null)
        {
            _context.Families.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Family));
        }
    }
}
