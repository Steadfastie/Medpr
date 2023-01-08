using AutoMapper;
using MediatR;
using MedprCQS.Commands.Doctors;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.CommandHandlers.Doctors;

public class PatchDoctorCommandHandler : IRequestHandler<PatchDoctorCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public PatchDoctorCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doctor>(request.Doctor);

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