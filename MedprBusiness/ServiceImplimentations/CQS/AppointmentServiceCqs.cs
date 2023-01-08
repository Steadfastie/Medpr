using AutoMapper;
using MediatR;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Appointments;
using MedprCQS.Queries.Appointments;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class AppointmentServiceCqs : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AppointmentServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<AppointmentDTO> GetAppointmentByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetAppointmentByIdQuery()
        {
            Id = id
        });
    }

    public async Task<List<AppointmentDTO>> GetAppointmentsByUserIdAsync(Guid id)
    {
        return await _mediator.Send(new GetAppointmentsByUserIdQuery()
        {
            UserId = id
        });
    }

    public async Task<List<AppointmentDTO>> GetAllAppointmentsAsync()
    {
        return await _mediator.Send(new GetAllAppointmentsQuery());
    }

    public async Task<int> CreateAppointmentAsync(AppointmentDTO dto)
    {
        return await _mediator.Send(new CreateAppointmentCommand()
        {
            Appointment = dto
        });
    }

    public async Task<int> PatchAppointmentAsync(Guid id, List<PatchModel> patchList)
    {
        var appointment = await _mediator.Send(new GetAppointmentByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchAppointmentCommand()
        {
            Appointment = appointment,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteAppointmentAsync(AppointmentDTO dto)
    {
        return await _mediator.Send(new DeleteAppointmentCommand() { Appointment = dto });
    }
}