using AutoMapper;
using MediatR;
using MedprCQS.Commands.Doctors;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCQS.Handlers.CommandHandlers.Doctors;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateDoctorCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<int> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doctor>(request.Doctor);

        if (entity != null)
        {
            await _context.Doctors.AddAsync(entity, cancellationToken);
            
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Doctor));
        }
    }
}
