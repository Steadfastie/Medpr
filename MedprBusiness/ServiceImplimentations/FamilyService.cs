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

public class FamilyService : IFamilyService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FamilyService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<FamilyDTO> GetFamiliesByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Families.GetByIdAsync(id);
        var dto = _mapper.Map<FamilyDTO>(entity);

        return dto;
    }

    public async Task<List<FamilyDTO>> GetAllFamiliesAsync()
    {
        var entities = await _unitOfWork.Families.GetAllAsync();
        var dtos = _mapper.Map<List<FamilyDTO>>(entities);

        return dtos;
    }

    public Task<List<FamilyDTO>> GetFamiliesByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.Families
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .OrderBy(Family => Family.Surname)
            .Select(Family => _mapper.Map<FamilyDTO>(Family))
            .ToListAsync();
        return list;
    }

    public async Task<List<FamilyDTO>> GetFamiliesRelevantToUser(Guid id)
    {
        var list = await _unitOfWork.FamilyMembers
            .FindBy(member => member.UserId == id)
            .Select(member => member.FamilyId)
            .ToListAsync();
        var dtos = await _unitOfWork.Families
            .FindBy(family => list.Contains(family.Id))
            .Select(family => _mapper.Map<FamilyDTO>(family))
            .ToListAsync();
        return dtos;
    }

    public async Task<int> CreateFamilyAsync(FamilyDTO dto)
    {
        var entity = _mapper.Map<Family>(dto);

        if (entity != null)
        {
            await _unitOfWork.Families.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchFamilyAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.Families.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteFamilyAsync(FamilyDTO dto)
    {
        var entity = _mapper.Map<Family>(dto);

        if (entity != null)
        {
            _unitOfWork.Families.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
