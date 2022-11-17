using AutoMapper;
using MediatR;
using MedprCQS.Commands.Vaccinations;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Vaccinations;

public class PatchVaccinationCommandHandler : IRequestHandler<PatchVaccinationCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public PatchVaccinationCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(PatchVaccinationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vaccination>(request.Vaccination);

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
