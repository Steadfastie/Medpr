using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IPrescriptionService
{
    Task<List<PrescriptionDTO>> GetPrescriptionsByPageNumberAndPageSizeAsync
        (int pageNumber, int pageSize);
    Task<List<PrescriptionDTO>> GetPrescriptionsRelevantToUser(Guid id);

    Task<PrescriptionDTO> GetPrescriptionsByIdAsync(Guid id);

    Task<int> CreatePrescriptionAsync(PrescriptionDTO dto);
    Task<int> PatchPrescriptionAsync(Guid id, List<PatchModel> patchList);
    Task<int> DeletePrescriptionAsync(PrescriptionDTO dto);
}