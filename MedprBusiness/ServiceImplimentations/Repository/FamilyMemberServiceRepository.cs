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

public class FamilyMemberServiceRepository : IFamilyMemberService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FamilyMemberServiceRepository(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<FamilyMemberDTO> GetFamilyMemberByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.FamilyMembers.GetByIdAsync(id);
        var dto = _mapper.Map<FamilyMemberDTO>(entity);

        return dto;
    }

    public async Task<List<FamilyMemberDTO>> GetAllFamilyMembersAsync()
    {
        var list = _unitOfWork.FamilyMembers.Get();
        return await list.Select(member => _mapper.Map<FamilyMemberDTO>(member)).ToListAsync();
    }

    public async Task<List<FamilyMemberDTO>> GetMembersRelevantToFamily(Guid id)
    {
        var list = await _unitOfWork.FamilyMembers
            .FindBy(member => member.FamilyId == id)
            .Select(member => _mapper.Map<FamilyMemberDTO>(member))
            .ToListAsync();
        return list;
    }

    public async Task<bool> GetRoleByFamilyIdAndUserId(Guid familyId, Guid userId)
    {
        var isAdmin = await _unitOfWork.FamilyMembers
            .FindBy(member => member.FamilyId == familyId && member.UserId == userId)
            .Select(member => member.IsAdmin)
            .FirstOrDefaultAsync();
        return isAdmin;
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

    public async Task DeleteMemberFromDBAsync(Guid userId)
    {
        var members = _unitOfWork.FamilyMembers
            .FindBy(member => member.UserId == userId)
            .ToList();

        foreach (var member in members)
        {
            _unitOfWork.FamilyMembers.Remove(member);
        }

        await _unitOfWork.Commit();
    }
}
