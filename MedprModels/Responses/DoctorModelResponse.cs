using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class DoctorModelResponse : IHateoas
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Experience { get; set; }
    public List<Link>? Links { get; set; }
}