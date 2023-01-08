using AutoMapper;
using MediatR;
using MedprCQS.Commands.Appointments;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.CommandHandlers.Appointments;

public class PatchAppointmentCommandHandler : IRequestHandler<PatchAppointmentCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public PatchAppointmentCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchAppointmentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Appointment>(request.Appointment);

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