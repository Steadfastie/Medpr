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

namespace MedprBusiness.ServiceImplimentations.Repository;

public class DrugServiceRepository : IDrugService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DrugServiceRepository(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DrugDTO> GetDrugByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Drugs.GetByIdAsync(id);
        var dto = _mapper.Map<DrugDTO>(entity);

        return dto;
    }

    public async Task<DrugDTO?> GetDrugByNameAsync(string name)
    {
        var entity = await _unitOfWork.Drugs.FindBy(drug => drug.Name.Equals(name)).FirstOrDefaultAsync();
        return _mapper.Map<DrugDTO>(entity);
    }

    public async Task<List<DrugDTO>> GetAllDrugsAsync()
    {
        var entities = await _unitOfWork.Drugs.GetAllAsync();
        var dtos = _mapper.Map<List<DrugDTO>>(entities);

        return dtos;
    }

    public async Task<int> CreateDrugAsync(DrugDTO dto)
    {
        var entity = _mapper.Map<Drug>(dto);

        if (entity != null)
        {
            await _unitOfWork.Drugs.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchDrugAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Drugs.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteDrugAsync(DrugDTO dto)
    {
        var entity = _mapper.Map<Drug>(dto);

        if (entity != null)
        {
            _unitOfWork.Drugs.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
