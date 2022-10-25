using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IVaccineService
    {
        Task<List<VaccineDTO>> GetAllVaccines();

        Task<VaccineDTO> GetVaccinesByIdAsync(Guid id);
        Task<List<VaccineDTO>> GetAllVaccinesAsync();

        Task<int> CreateVaccineAsync(VaccineDTO dto);
        Task<int> PatchVaccineAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteVaccineAsync(VaccineDTO dto);
    }
}