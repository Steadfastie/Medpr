using AutoMapper;
using MediatR;
using MedprCQS.Commands.Appointments;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.Appointments;

public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteAppointmentCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Appointment>(request.Appointment);

        if (entity != null)
        {
            _context.Appointments.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Appointment));
        }
    }
}