using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Families;
using MedprCQS.Commands.FamilyMembers;
using MedprCQS.Queries.Families;
using MedprCQS.Queries.FamilyMembers;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class FamilyServiceCqs : IFamilyService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public FamilyServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<FamilyDTO> GetFamilyByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetFamilyByIdQuery()
        {
            Id = id
        });
    }

    public async Task<List<FamilyDTO>> GetFamiliesBySubstringAsync(string substring)
    {
        return await _mediator.Send(new GetFamilyBySubstringQuery()
        {
            Substring = substring
        });
    }

    public async Task<List<FamilyDTO>> GetAllFamiliesAsync()
    {
        return await _mediator.Send(new GetAllFamiliesQuery());
    }

    public async Task<List<FamilyDTO>> GetFamiliesRelevantToUser(Guid userId)
    {
        var familyList = await _mediator.Send(new GetFamiliesRelevantToUserQuery()
        {
            UserId = userId,
        });

        return familyList;
    }

    public async Task<int> CreateFamilyAsync(FamilyDTO dto)
    {
        return await _mediator.Send(new CreateFamilyCommand()
        {
            Family = dto
        });
    }

    public async Task<int> PatchFamilyAsync(Guid id, List<PatchModel> patchList)
    {
        var family = await _mediator.Send(new GetFamilyByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchFamilyCommand()
        {
            Family = family,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteFamilyAsync(FamilyDTO dto)
    {
        return await _mediator.Send(new DeleteFamilyCommand() { Family = dto });
    }

    public async Task DeleteAllCreatedFamilies(Guid userId)
    {
        var families = await _mediator.Send(new GetFamiliesByCreatorIdQuery()
        {
            CreatorId = userId
        });

        foreach (var family in families)
        {
            var members = await _mediator.Send(new GetFamilyMembersByFamilyIdQuery()
            {
                FamilyId = family.Id
            });

            foreach (var member in members)
            {
                await _mediator.Send(new DeleteFamilyMemberCommand()
                {
                    FamilyMember = member
                });
            }

            await _mediator.Send(new DeleteFamilyCommand()
            {
                Family = family
            });
        }
    }
}