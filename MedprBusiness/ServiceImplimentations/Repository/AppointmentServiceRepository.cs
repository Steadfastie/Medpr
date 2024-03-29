﻿using AutoMapper;
using MedprAbstractions;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedprBusiness.ServiceImplimentations.Repository;

public class AppointmentServiceRepository : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentServiceRepository(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppointmentDTO> GetAppointmentByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Appointments.GetByIdAsync(id);
        var dto = _mapper.Map<AppointmentDTO>(entity);

        return dto;
    }

    public Task<List<AppointmentDTO>> GetAppointmentsByUserIdAsync(Guid id)
    {
        var list = _unitOfWork.Appointments
            .FindBy(appointment => appointment.UserId == id)
            .Select(appointment => _mapper.Map<AppointmentDTO>(appointment))
            .ToListAsync();
        return list;
    }

    public async Task<List<AppointmentDTO>> GetAllAppointmentsAsync()
    {
        var list = await _unitOfWork.Appointments.Get().ToListAsync();
        return list.Select(appointment => _mapper.Map<AppointmentDTO>(appointment)).ToList();
    }

    public async Task<int> CreateAppointmentAsync(AppointmentDTO dto)
    {
        var entity = _mapper.Map<Appointment>(dto);

        if (entity != null)
        {
            await _unitOfWork.Appointments.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchAppointmentAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Appointments.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteAppointmentAsync(AppointmentDTO dto)
    {
        var entity = _mapper.Map<Appointment>(dto);

        if (entity != null)
        {
            _unitOfWork.Appointments.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}