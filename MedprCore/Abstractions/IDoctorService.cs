using AspNetSample.Core;
using MedprCore.DTO;

namespace MedprCore.Abstractions
{
    public interface IDoctorService
    {
        Task<List<DoctorDTO>> GetDoctorsByPageNumberAndPageSizeAsync
            (int pageNumber, int pageSize);

        Task<DoctorDTO> GetDoctorsByIdAsync(Guid id);

        Task<int> CreateDoctorAsync(DoctorDTO dto);
        Task<int> PatchDoctorAsync(Guid id, List<PatchModel> patchList);
        Task<int> DeleteDoctorAsync(DoctorDTO dto);
    }
}