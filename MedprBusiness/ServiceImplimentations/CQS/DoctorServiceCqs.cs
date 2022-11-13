using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MedprAbstractions;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprCQS.Commands.Doctors;
using MedprCQS.Queries.Doctors;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedprBusiness.ServiceImplimentations.CQS;

public class DoctorServiceCqs : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public DoctorServiceCqs(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<DoctorDTO> GetDoctorByIdAsync(Guid id)
    {
        return await _mediator.Send(new GetDoctorByIdQuery()
        {
            Id = id
        });
    }

    public async Task<DoctorDTO> GetDoctorByNameAsync(string name)
    {
        return await _mediator.Send(new GetDoctorByNameQuery()
        {
            Name = name
        });
    }

    public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
    {
        return await _mediator.Send(new GetAllDoctorsQuery());
    }

    public async Task<int> CreateDoctorAsync(DoctorDTO dto)
    {
        return await _mediator.Send(new CreateDoctorCommand()
        {
            Doctor = dto
        });
    }

    public async Task<int> PatchDoctorAsync(Guid id, List<PatchModel> patchList)
    {
        var drug = await _mediator.Send(new GetDoctorByIdQuery()
        {
            Id = id
        });

        return await _mediator.Send(new PatchDoctorCommand()
        {
            Doctor = drug,
            PatchList = patchList
        });
    }

    public async Task<int> DeleteDoctorAsync(DoctorDTO dto)
    {
        return await _mediator.Send(new DeleteDoctorCommand() { Doctor = dto });
    }
}
