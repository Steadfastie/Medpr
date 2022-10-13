using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IFamilyMemberService
{
    Task<List<FamilyMemberDTO>> GetFamilyMembersByPageNumberAndPageSizeAsync
        (int pageNumber, int pageSize);

    Task<FamilyMemberDTO> GetFamilyMembersByIdAsync(Guid id);

    Task<List<FamilyDTO>> GetFamiliesRelevantToUser(Guid id);
    Task<List<UserDTO>> GetUsersRelevantToFamily(Guid id);

    Task<List<FamilyMemberDTO>> GetMembersRelevantToFamily(Guid id);

    Task<int> CreateFamilyMemberAsync(FamilyMemberDTO dto);
    Task<int> PatchFamilyMemberAsync(Guid id, List<PatchModel> patchList);
    Task<int> DeleteFamilyMemberAsync(FamilyMemberDTO dto);
}