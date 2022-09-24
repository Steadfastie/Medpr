using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDrugService
    {
        Task<List<DrugDTO>> GetDrugsByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);

        Task<List<DrugDTO>> GetNewArticlesFromExternalSourcesAsync();

        Task<DrugDTO> GetDrugsByIdAsync(Guid id);

        Task<int> CreateArticleAsync(DrugDTO dto);
    }
}