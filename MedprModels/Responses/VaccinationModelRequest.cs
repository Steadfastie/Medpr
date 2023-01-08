using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class VaccinationModelResponse : IHateoas
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int DaysOfProtection { get; set; }
    public UserModelResponse User { get; set; }
    public VaccineModelResponse Vaccine { get; set; }
    public List<Link>? Links { get; set; }
}