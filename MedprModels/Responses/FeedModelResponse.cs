namespace MedprModels.Responses;

public class FeedModelResponse
{
    public List<AppointmentModelResponse>? Appointments { get; set; }
    public List<VaccinationModelResponse>? Vaccinations { get; set; }
    public List<PrescriptionModelResponse>? Prescriptions { get; set; }
}