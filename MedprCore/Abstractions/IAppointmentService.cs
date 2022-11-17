using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IAppointmentService
{
    Task<List<AppointmentDTO>> GetAllAppointmentsAsync();
    Task<List<AppointmentDTO>> GetAppointmentsByUserIdAsync(Guid id);
    Task<AppointmentDTO> GetAppointmentByIdAsync(Guid id);
    Task<int> CreateAppointmentAsync(AppointmentDTO dto);
    Task<int> PatchAppointmentAsync(Guid id, List<PatchModel> patchList);
    Task<int> DeleteAppointmentAsync(AppointmentDTO dto);
}