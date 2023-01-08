using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDoctorService
    {
        Task<DoctorDTO> GetDoctorByIdAsync(Guid id);

        Task<DoctorDTO> GetDoctorByNameAsync(string name);

        Task<List<DoctorDTO>> GetAllDoctorsAsync();

        Task<int> CreateDoctorAsync(DoctorDTO dto);

        Task<int> PatchDoctorAsync(Guid id, List<PatchModel> patchList);

        Task<int> DeleteDoctorAsync(DoctorDTO dto);
    }
}