using AutoMapper;
using MediatR;
using MedprCQS.Commands.Families;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.CommandHandlers.Families;

public class PatchFamilyCommandHandler : IRequestHandler<PatchFamilyCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public PatchFamilyCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchFamilyCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Family>(request.Family);

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