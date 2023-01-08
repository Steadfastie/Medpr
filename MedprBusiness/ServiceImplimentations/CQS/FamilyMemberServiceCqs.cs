using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.FamilyMembers;
using MedprCQS.Queries.FamilyMembers;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class FamilyMemberServiceCqs : IFamilyMemberService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public FamilyMemberServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<FamilyMemberDTO> GetFamilyMemberByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetFamilyMemberByIdQuery()
        {
            Id = id
        });
    }

    public async Task<List<FamilyMemberDTO>> GetAllFamilyMembersAsync()
    {
        return await _mediator.Send(new GetAllFamilyMembersQuery());
    }

    public async Task<List<FamilyMemberDTO>> GetMembersRelevantToFamily(Guid id)
    {
        return await _mediator.Send(new GetFamilyMembersByFamilyIdQuery()
        {
            FamilyId = id
        });
    }

    public async Task<bool> GetRoleByFamilyIdAndUserId(Guid familyId, Guid userId)
    {
        var member = await _mediator.Send(new GetFamilyMembersByFamilyIdAndUserIdQuery()
        {
            FamilyId = familyId,
            UserId = userId
        });

        return member.IsAdmin;
    }

    public async Task<int> CreateFamilyMemberAsync(FamilyMemberDTO dto)
    {
        return await _mediator.Send(new CreateFamilyMemberCommand()
        {
            FamilyMember = dto
        });
    }

    public async Task<int> PatchFamilyMemberAsync(Guid id, List<PatchModel> patchList)
    {
        var member = await _mediator.Send(new GetFamilyMemberByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchFamilyMemberCommand()
        {
            FamilyMember = member,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteFamilyMemberAsync(FamilyMemberDTO dto)
    {
        return await _mediator.Send(new DeleteFamilyMemberCommand() { FamilyMember = dto });
    }

    public async Task DeleteMemberFromDBAsync(Guid userId)
    {
        var members = await _mediator.Send(new GetFamilyMembersByUserIdQuery()
        {
            UserId = userId
        });

        foreach (var member in members)
        {
            await _mediator.Send(new DeleteFamilyMemberCommand() { FamilyMember = member });
        }
    }
}