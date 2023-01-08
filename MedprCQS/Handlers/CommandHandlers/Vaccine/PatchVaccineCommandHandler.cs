using AutoMapper;
using MediatR;
using MedprCQS.Commands.Vaccines;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.CommandHandlers.Vaccines;

public class PatchVaccineCommandHandler : IRequestHandler<PatchVaccineCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public PatchVaccineCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchVaccineCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vaccine>(request.Vaccine);

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