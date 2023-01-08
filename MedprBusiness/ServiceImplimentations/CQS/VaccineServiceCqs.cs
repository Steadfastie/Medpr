using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Vaccines;
using MedprCQS.Queries.Vaccines;

namespace MedprBusiness.ServiceImplimentations.CQS;

public class VaccineServiceCqs : IVaccineService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public VaccineServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<VaccineDTO> GetVaccineByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetVaccineByIdQuery()
        {
            Id = id
        });
    }

    public async Task<VaccineDTO> GetVaccineByNameAsync(string name)
    {
        return await _mediator.Send(new GetVaccineByNameQuery()
        {
            Name = name
        });
    }

    public async Task<List<VaccineDTO>> GetAllVaccinesAsync()
    {
        return await _mediator.Send(new GetAllVaccinesQuery());
    }

    public async Task<int> CreateVaccineAsync(VaccineDTO dto)
    {
        return await _mediator.Send(new CreateVaccineCommand()
        {
            Vaccine = dto
        });
    }

    public async Task<int> PatchVaccineAsync(Guid id, List<PatchModel> patchList)
    {
        var vaccine = await _mediator.Send(new GetVaccineByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchVaccineCommand()
        {
            Vaccine = vaccine,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteVaccineAsync(VaccineDTO dto)
    {
        return await _mediator.Send(new DeleteVaccineCommand() { Vaccine = dto });
    }
}