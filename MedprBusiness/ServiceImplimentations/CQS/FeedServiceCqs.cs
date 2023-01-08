using AutoMapper;
using MediatR;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Queries.Appointments;

namespace MedprBusiness.ServiceImplimentations.Cqs;

public class FeedServiceCqs : IFeedService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public FeedServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<List<AppointmentDTO>> GetUpcomingAppointmentsByUserIdAsync(Guid userId)
    {
        var appointments = await _mediator.Send(new GetUpcomingAppointmentsByUserIdQuery()
        {
            UserId = userId,
            Date = DateTime.UtcNow
        });

        if (appointments.Count > 3)
        {
            appointments.Sort((appointment1, appointment2) => DateTime.Compare(appointment1.Date, appointment2.Date));
            return appointments.Take(3).ToList();
        }
        else
        {
            return appointments;
        }
    }

    public async Task<List<VaccinationDTO>> GetUpcomingVaccinationsByUserIdAsync(Guid userId)
    {
        var vaccinations = await _mediator.Send(new GetUpcomingVaccinationsByUserIdQuery()
        {
            UserId = userId,
            Date = DateTime.UtcNow
        });

        if (vaccinations.Count > 3)
        {
            vaccinations.Sort((vaccination1, vaccination2) => DateTime.Compare(vaccination1.Date, vaccination2.Date));
            return vaccinations.Take(3).ToList();
        }
        else
        {
            return vaccinations;
        }
    }

    public async Task<List<PrescriptionDTO>> GetUpcomingPrescriptionsByUserIdAsync(Guid userId)
    {
        var prescriptions = await _mediator.Send(new GetUpcomingPrescriptionsByUserIdQuery()
        {
            UserId = userId,
            Date = DateTime.UtcNow
        });

        if (prescriptions.Count > 3)
        {
            prescriptions.Sort((prescription1, prescription2) => DateTime.Compare(prescription1.Date, prescription2.Date));
            return prescriptions.Take(3).ToList();
        }
        else
        {
            return prescriptions;
        }
    }

    public async Task<List<PrescriptionDTO>> GetOngoingPrescriptionsByUserIdAsync(Guid userId)
    {
        var prescriptions = await _mediator.Send(new GetOngoingPrescriptionsByUserIdQuery()
        {
            UserId = userId,
            Date = DateTime.UtcNow
        });

        if (prescriptions.Count > 3)
        {
            prescriptions.Sort((prescription1, prescription2) => DateTime.Compare(prescription1.EndDate, prescription2.EndDate));
            return prescriptions.Take(3).ToList();
        }
        else
        {
            return prescriptions;
        }
    }
}