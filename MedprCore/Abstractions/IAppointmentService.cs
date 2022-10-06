using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IAppointmentService
{
    Task<List<AppointmentDTO>> GetAppointmentsByPageNumberAndPageSizeAsync
        (int pageNumber, int pageSize);

    Task<AppointmentDTO> GetAppointmentsByIdAsync(Guid id);

    Task<int> CreateAppointmentAsync(AppointmentDTO dto);
    Task<int> PatchAppointmentAsync(Guid id, List<PatchModel> patchList);
    Task<int> DeleteAppointmentAsync(AppointmentDTO dto);
}