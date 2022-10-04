using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDrugService
    {
        Task<List<DrugDTO>> GetDrugsByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);

        Task<DrugDTO> GetDrugsByIdAsync(Guid id);

        Task<int> CreateDrugAsync(DrugDTO dto);
        Task<int> PatchDrugAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteDrugAsync(DrugDTO dto);
    }
}