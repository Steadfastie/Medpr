using AutoMapper;
using MediatR;
using MedprCQS.Commands.Doctors;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.Doctors;

public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteDoctorCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doctor>(request.Doctor);

        if (entity != null)
        {
            _context.Doctors.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Doctor));
        }
    }
}