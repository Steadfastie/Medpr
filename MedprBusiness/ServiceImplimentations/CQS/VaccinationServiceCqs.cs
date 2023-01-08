using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Vaccinations;
using MedprCQS.Queries.Vaccinations;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class VaccinationServiceCqs : IVaccinationService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public VaccinationServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<VaccinationDTO> GetVaccinationByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetVaccinationByIdQuery()
        {
            Id = id
        });
    }

    public async Task<List<VaccinationDTO>> GetVaccinationsByUserIdAsync(Guid id)
    {
        return await _mediator.Send(new GetVaccinationsByUserIdQuery()
        {
            UserId = id
        });
    }

    public async Task<List<VaccinationDTO>> GetAllVaccinationsAsync()
    {
        return await _mediator.Send(new GetAllVaccinationsQuery());
    }

    public async Task<int> CreateVaccinationAsync(VaccinationDTO dto)
    {
        return await _mediator.Send(new CreateVaccinationCommand()
        {
            Vaccination = dto
        });
    }

    public async Task<int> PatchVaccinationAsync(Guid id, List<PatchModel> patchList)
    {
        var vaccination = await _mediator.Send(new GetVaccinationByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchVaccinationCommand()
        {
            Vaccination = vaccination,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteVaccinationAsync(VaccinationDTO dto)
    {
        return await _mediator.Send(new DeleteVaccinationCommand() { Vaccination = dto });
    }
}