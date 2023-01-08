using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class AppointmentModelResponse : IHateoas
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Place { get; set; }
    public UserModelResponse User { get; set; }
    public DoctorModelResponse Doctor { get; set; }
    public List<Link>? Links { get; set; }
}