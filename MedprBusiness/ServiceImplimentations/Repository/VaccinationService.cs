﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedprAbstractions;
using MedprCore;
using MedprCore.Abstractions;
using MedprCore.DTO;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedprBusiness.ServiceImplimentations.Repository;

public class VaccinationService : IVaccinationService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VaccinationService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<VaccinationDTO> GetVaccinationsByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Vaccinations.GetByIdAsync(id);
        var dto = _mapper.Map<VaccinationDTO>(entity);

        return dto;
    }

    public async Task<List<VaccinationDTO>> GetAllVaccinations()
    {
        var dtos = _unitOfWork.Vaccinations.Get();
        return await dtos.Select(vaccination => _mapper.Map<VaccinationDTO>(vaccination)).ToListAsync();
    }

    public Task<List<VaccinationDTO>> GetVaccinationsRelevantToUser(Guid id)
    {
        var list = _unitOfWork.Vaccinations
            .FindBy(vaccination => vaccination.UserId == id)
            .Select(vaccination => _mapper.Map<VaccinationDTO>(vaccination))
            .ToListAsync();
        return list;
    }

    public async Task<int> CreateVaccinationAsync(VaccinationDTO dto)
    {
        var entity = _mapper.Map<Vaccination>(dto);

        if (entity != null)
        {
            await _unitOfWork.Vaccinations.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchVaccinationAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Vaccinations.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteVaccinationAsync(VaccinationDTO dto)
    {
        var entity = _mapper.Map<Vaccination>(dto);

        if (entity != null)
        {
            _unitOfWork.Vaccinations.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }


}