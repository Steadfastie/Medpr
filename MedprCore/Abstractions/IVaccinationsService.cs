using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IVaccinationService
    {
        Task<List<VaccinationDTO>> GetVaccinationsByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);

        Task<List<VaccinationDTO>> GetVaccinationsRelevantToUser(Guid id);

        Task<VaccinationDTO> GetVaccinationsByIdAsync(Guid id);

        Task<int> CreateVaccinationAsync(VaccinationDTO dto);
        Task<int> PatchVaccinationAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteVaccinationAsync(VaccinationDTO dto);
    }
}