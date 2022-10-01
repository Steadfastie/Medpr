using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetSample.Core;
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

    public Task<List<DoctorDTO>> GetDoctorsByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.Doctors
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .OrderBy(Doctor => Doctor.Name)
            .Select(Doctor => _mapper.Map<DoctorDTO>(Doctor))
            .ToListAsync();
        return list;
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
