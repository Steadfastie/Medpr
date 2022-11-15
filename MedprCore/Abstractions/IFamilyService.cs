using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IFamilyService
    {
        Task<List<FamilyDTO>> GetFamiliesRelevantToUser(Guid userId);
        Task<FamilyDTO> GetFamilyByIdAsync(Guid id);
        Task<List<FamilyDTO>> GetAllFamiliesAsync();
        Task<int> CreateFamilyAsync(FamilyDTO dto);
        Task<int> PatchFamilyAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteFamilyAsync(FamilyDTO dto);
        Task DeleteAllCreatedFamilies(Guid userId);
    }
}