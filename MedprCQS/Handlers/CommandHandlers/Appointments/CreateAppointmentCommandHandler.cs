using AutoMapper;
using MediatR;
using MedprCQS.Commands.Appointments;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Appointments;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateAppointmentCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Appointment>(request.Appointment);

        if (entity != null)
        {
            await _context.Appointments.AddAsync(entity, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Appointment));
        }
    }
}
