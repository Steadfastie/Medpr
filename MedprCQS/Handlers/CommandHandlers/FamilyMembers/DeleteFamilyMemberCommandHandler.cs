using AutoMapper;
using MediatR;
using MedprCQS.Commands.FamilyMembers;
using MedprDB;
using MedprDB.Entities;

namespace MedprCQS.Handlers.CommandHandlers.FamilyMembers;

public class DeleteFamilyMemberCommandHandler : IRequestHandler<DeleteFamilyMemberCommand, int>
{
    private readonly MedprDBContext _context;
    private readonly IMapper _mapper;

    public DeleteFamilyMemberCommandHandler(MedprDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteFamilyMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<FamilyMember>(request.FamilyMember);

        if (entity != null)
        {
            _context.FamilyMembers.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException(nameof(request.FamilyMember));
        }
    }
}