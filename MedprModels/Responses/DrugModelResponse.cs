using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class DrugModelResponse : IHateoas
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PharmGroup { get; set; }
    public int Price { get; set; }
    public List<Link>? Links { get; set; }
}