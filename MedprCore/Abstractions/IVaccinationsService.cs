using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IVaccinationService
    {
        Task<List<VaccinationDTO>> GetAllVaccinationsAsync();

        Task<List<VaccinationDTO>> GetVaccinationsByUserIdAsync(Guid id);

        Task<VaccinationDTO> GetVaccinationByIdAsync(Guid id);

        Task<int> CreateVaccinationAsync(VaccinationDTO dto);

        Task<int> PatchVaccinationAsync(Guid id, List<PatchModel> patchList);

        Task<int> DeleteVaccinationAsync(VaccinationDTO dto);
    }
}