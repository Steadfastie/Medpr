using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDrugService
    {
        Task<DrugDTO> GetDrugByIdAsync(Guid id);
        Task<DrugDTO> GetDrugByNameAsync(string name);
        Task<List<DrugDTO>> GetAllDrugsAsync();
        Task<int> CreateDrugAsync(DrugDTO dto);
        Task<int> PatchDrugAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteDrugAsync(DrugDTO dto);
    }
}