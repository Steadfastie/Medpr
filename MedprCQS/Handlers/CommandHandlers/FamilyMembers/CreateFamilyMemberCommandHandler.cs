using AutoMapper;
using MediatR;
using MedprCQS.Commands.FamilyMembers;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.FamilyMembers;

public class CreateFamilyMemberCommandHandler : IRequestHandler<CreateFamilyMemberCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public CreateFamilyMemberCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<FamilyMember>(request.FamilyMember);

        if (entity != null)
        {
            await _context.FamilyMembers.AddAsync(entity, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.FamilyMember));
        }
    }
}