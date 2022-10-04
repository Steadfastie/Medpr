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

public class VaccineService : IVaccineService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VaccineService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<VaccineDTO> GetVaccinesByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Vaccines.GetByIdAsync(id);
        var dto = _mapper.Map<VaccineDTO>(entity);

        return dto;
    }

    public async Task<List<VaccineDTO>> GetAllVaccinesAsync()
    {
        var entities = await _unitOfWork.Vaccines.GetAllAsync();
        var dtos = _mapper.Map<List<VaccineDTO>>(entities);

        return dtos;
    }

    public Task<List<VaccineDTO>> GetVaccinesByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.Vaccines
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .OrderBy(Vaccine => Vaccine.Name)
            .Select(Vaccine => _mapper.Map<VaccineDTO>(Vaccine))
            .ToListAsync();
        return list;
    }

    public async Task<int> CreateVaccineAsync(VaccineDTO dto)
    {
        var entity = _mapper.Map<Vaccine>(dto);

        if (entity != null)
        {
            await _unitOfWork.Vaccines.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchVaccineAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Vaccines.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteVaccineAsync(VaccineDTO dto)
    {
        var entity = _mapper.Map<Vaccine>(dto);

        if (entity != null)
        {
            _unitOfWork.Vaccines.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
