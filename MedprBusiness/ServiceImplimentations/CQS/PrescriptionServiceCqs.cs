using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Prescriptions;
using MedprCQS.Queries.Prescriptions;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class PrescriptionServiceCqs : IPrescriptionService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public PrescriptionServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<PrescriptionDTO> GetPrescriptionByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetPrescriptionByIdQuery()
        {
            Id = id
        });
    }

    public async Task<List<PrescriptionDTO>> GetPrescriptionsByUserIdAsync(Guid id)
    {
        return await _mediator.Send(new GetPrescriptionsByUserIdQuery()
        {
            UserId = id
        });
    }

    public async Task<List<PrescriptionDTO>> GetAllPrescriptionsAsync()
    {
        return await _mediator.Send(new GetAllPrescriptionsQuery());
    }

    public async Task<int> CreatePrescriptionAsync(PrescriptionDTO dto)
    {
        return await _mediator.Send(new CreatePrescriptionCommand()
        {
            Prescription = dto
        });
    }

    public async Task<int> PatchPrescriptionAsync(Guid id, List<PatchModel> patchList)
    {
        var prescription = await _mediator.Send(new GetPrescriptionByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchPrescriptionCommand()
        {
            Prescription = prescription,
            PatchList = patchList
        });
    }

    public async Task<int> DeletePrescriptionAsync(PrescriptionDTO dto)
    {
        return await _mediator.Send(new DeletePrescriptionCommand() { Prescription = dto });
    }
}