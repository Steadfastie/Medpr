using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class PrescriptionModelResponse : IHateoas
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime EndDate { get; set; }
    public int Dose { get; set; }
    public UserModelResponse User { get; set; }
    public DoctorModelResponse Doctor { get; set; }
    public DrugModelResponse Drug { get; set; }
    public List<Link>? Links { get; set; }
}