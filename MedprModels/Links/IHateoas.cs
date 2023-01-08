using MedprModels.Links;

namespace MedprModels.Interfaces;

public interface IHateoas
{
    public Guid Id { get; set; }
    public List<Link>? Links { get; set; }
}