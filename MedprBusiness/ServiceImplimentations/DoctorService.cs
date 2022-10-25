using System;
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

namespace MedprBusiness.ServiceImplementations;

public class DoctorService : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DoctorDTO> GetDoctorsByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Doctors.GetByIdAsync(id);
        var dto = _mapper.Map<DoctorDTO>(entity);

        return dto;
    }

    public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
    {
        var entities = await _unitOfWork.Doctors.GetAllAsync();
        var dtos = _mapper.Map<List<DoctorDTO>>(entities);

        return dtos;
    }

    public async Task<List<DoctorDTO>> GetAllDoctors()
    {
        var list = _unitOfWork.Doctors.Get();
        return await list.Select(doctor => _mapper.Map<DoctorDTO>(doctor)).ToListAsync();
    }

    public async Task<int> CreateDoctorAsync(DoctorDTO dto)
    {
        var entity = _mapper.Map<Doctor>(dto);

        if (entity != null)
        {
            await _unitOfWork.Doctors.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchDoctorAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Doctors.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteDoctorAsync(DoctorDTO dto)
    {
        var entity = _mapper.Map<Doctor>(dto);

        if (entity != null)
        {
            _unitOfWork.Doctors.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
