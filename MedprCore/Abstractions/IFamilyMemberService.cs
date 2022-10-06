using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IFamilyMemberService
{
    Task<List<FamilyMemberDTO>> GetFamilyMembersByPageNumberAndPageSizeAsync
        (int pageNumber, int pageSize);

    Task<FamilyMemberDTO> GetFamilyMembersByIdAsync(Guid id);

    Task<int> CreateFamilyMemberAsync(FamilyMemberDTO dto);
    Task<int> PatchFamilyMemberAsync(Guid id, List<PatchModel> patchList);
    Task<int> DeleteFamilyMemberAsync(FamilyMemberDTO dto);
}