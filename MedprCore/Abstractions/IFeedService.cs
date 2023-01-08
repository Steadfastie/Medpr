using MedprCore.DTO;

namespace MedprCore.Abstractions;

public interface IFeedService
{
    Task<List<AppointmentDTO>> GetUpcomingAppointmentsByUserIdAsync(Guid userId);

    Task<List<VaccinationDTO>> GetUpcomingVaccinationsByUserIdAsync(Guid userId);

    Task<List<PrescriptionDTO>> GetUpcomingPrescriptionsByUserIdAsync(Guid userId);

    Task<List<PrescriptionDTO>> GetOngoingPrescriptionsByUserIdAsync(Guid userId);
}