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

public class FamilyMemberService : IFamilyMemberService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FamilyMemberService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<FamilyMemberDTO> GetFamilyMembersByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.FamilyMembers.GetByIdAsync(id);
        var dto = _mapper.Map<FamilyMemberDTO>(entity);

        return dto;
    }

    public Task<List<FamilyMemberDTO>> GetFamilyMembersByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
    {
        var list = _unitOfWork.FamilyMembers
            .Get()
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .Select(familyMember => _mapper.Map<FamilyMemberDTO>(familyMember))
            .ToListAsync();
        return list;
    }

    public async Task<int> CreateFamilyMemberAsync(FamilyMemberDTO dto)
    {
        var entity = _mapper.Map<FamilyMember>(dto);

        if (entity != null)
        {
            await _unitOfWork.FamilyMembers.AddAsync(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }

    public async Task<int> PatchFamilyMemberAsync(Guid id, List<PatchModel> patchList)
    {
        await _unitOfWork.FamilyMembers.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteFamilyMemberAsync(FamilyMemberDTO dto)
    {
        var entity = _mapper.Map<FamilyMember>(dto);

        if (entity != null)
        {
            _unitOfWork.FamilyMembers.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(dto));
        }
    }
}
