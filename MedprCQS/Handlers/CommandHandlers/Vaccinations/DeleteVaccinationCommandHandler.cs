using AutoMapper;
using MediatR;
using MedprCQS.Commands.Vaccinations;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.Vaccinations;

public class DeleteVaccinationCommandHandler : IRequestHandler<DeleteVaccinationCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteVaccinationCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vaccination>(request.Vaccination);

        if (entity != null)
        {
            _context.Vaccinations.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Vaccination));
        }
    }
}