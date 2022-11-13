using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IVaccineService
    {
        Task<VaccineDTO> GetVaccineByIdAsync(Guid id);
        Task<VaccineDTO> GetVaccineByNameAsync(string name);
        Task<List<VaccineDTO>> GetAllVaccinesAsync();
        Task<int> CreateVaccineAsync(VaccineDTO dto);
        Task<int> PatchVaccineAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteVaccineAsync(VaccineDTO dto);
    }
}