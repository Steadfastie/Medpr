using AutoMapper;
using MediatR;
using MedprCQS.Commands.FamilyMembers;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.FamilyMembers;

public class PatchFamilyMemberCommandHandler : IRequestHandler<PatchFamilyMemberCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public PatchFamilyMemberCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(PatchFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<FamilyMember>(request.FamilyMember);

        var nameValuePropertiesPairs = request.PatchList
            .ToDictionary(
                patchModel => patchModel.PropertyName,
                patchModel => patchModel.PropertyValue);

        var dbEntityEntry = _context.Entry(entity);
        dbEntityEntry.CurrentValues.SetValues(nameValuePropertiesPairs);
        dbEntityEntry.State = EntityState.Modified;

        return await _context.SaveChangesAsync(cancellationToken);
    }
}
