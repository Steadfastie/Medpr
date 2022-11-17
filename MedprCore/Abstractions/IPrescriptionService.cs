using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IPrescriptionService
{
    Task<List<PrescriptionDTO>> GetAllPrescriptionsAsync();
    Task<List<PrescriptionDTO>> GetPrescriptionsByUserIdAsync(Guid id);
    Task<PrescriptionDTO> GetPrescriptionByIdAsync(Guid id);
    Task<int> CreatePrescriptionAsync(PrescriptionDTO dto);
    Task<int> PatchPrescriptionAsync(Guid id, List<PatchModel> patchList);
    Task<int> DeletePrescriptionAsync(PrescriptionDTO dto);
}