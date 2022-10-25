using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDoctorService
    {
        Task<List<DoctorDTO>> GetAllDoctors();

        Task<DoctorDTO> GetDoctorsByIdAsync(Guid id);

        Task<List<DoctorDTO>> GetAllDoctorsAsync();

        Task<int> CreateDoctorAsync(DoctorDTO dto);
        Task<int> PatchDoctorAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteDoctorAsync(DoctorDTO dto);
    }
}