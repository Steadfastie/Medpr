using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDrugService
    {
        Task<List<DrugDTO>> GetAllDrugs();
        Task<DrugDTO> GetDrugsByIdAsync(Guid id);
        Task<DrugDTO?> GetDrugsByNameAsync(string name);
        Task<List<DrugDTO>> GetAllDrugsAsync();
        Task<int> CreateDrugAsync(DrugDTO dto);
        Task<int> PatchDrugAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteDrugAsync(DrugDTO dto);
    }
}