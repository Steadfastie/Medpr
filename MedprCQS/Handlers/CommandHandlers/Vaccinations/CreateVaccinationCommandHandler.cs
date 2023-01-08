using AutoMapper;
using MediatR;
using MedprCQS.Commands.Vaccinations;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.Vaccinations;

public class CreateVaccinationCommandHandler : IRequestHandler<CreateVaccinationCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateVaccinationCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateVaccinationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Vaccination>(request.Vaccination);

        if (entity != null)
        {
            await _context.Vaccinations.AddAsync(entity, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.Vaccination));
        }
    }
}