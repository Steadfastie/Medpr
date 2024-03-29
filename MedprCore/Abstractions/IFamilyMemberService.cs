﻿using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IFamilyMemberService
{
    Task<List<FamilyMemberDTO>> GetAllFamilyMembersAsync();

    Task<FamilyMemberDTO> GetFamilyMemberByIdAsync(Guid id);

    Task<bool> GetRoleByFamilyIdAndUserId(Guid familyId, Guid userId);

    Task<List<FamilyMemberDTO>> GetMembersRelevantToFamily(Guid id);

    Task<int> CreateFamilyMemberAsync(FamilyMemberDTO dto);

    Task<int> PatchFamilyMemberAsync(Guid id, List<PatchModel> patchList);

    Task<int> DeleteFamilyMemberAsync(FamilyMemberDTO dto);

    Task DeleteMemberFromDBAsync(Guid userId);
}