using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Drugs;
using MedprCQS.Queries.Drugs;
using Microsoft.Extensions.Configuration;

namespace MedprBusiness.ServiceImplimentations.CQS;

public class DrugServiceCqs : IDrugService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public DrugServiceCqs(IMapper mapper,
        IMediator mediator,
        IConfiguration configuration)
    {
        _mapper = mapper;
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<DrugDTO> GetDrugByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetDrugByIdQuery()
        {
            Id = id
        });
    }

    public async Task<DrugDTO> GetDrugByNameAsync(string name)
    {
        return await _mediator.Send(new GetDrugByNameQuery()
        {
            Name = name
        });
    }

    public async Task<List<DrugDTO>> GetAllDrugsAsync()
    {
        return await _mediator.Send(new GetAllDrugsQuery());
    }

    public async Task<int> CreateDrugAsync(DrugDTO dto)
    {
        return await _mediator.Send(new CreateDrugCommand()
        {
            Drug = dto
        });
    }

    public async Task<int> PatchDrugAsync(Guid id, List<PatchModel> patchList)
    {
        var drug = await _mediator.Send(new GetDrugByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchDrugCommand()
        {
            Drug = drug,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteDrugAsync(DrugDTO dto)
    {
        return await _mediator.Send(new DeleteDrugCommand() { Drug = dto });
    }
}