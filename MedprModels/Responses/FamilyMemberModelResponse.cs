using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class FamilyMemberModelResponse : IHateoas
{
    public Guid Id { get; set; }
    public bool IsAdmin { get; set; }
    public UserModelResponse User { get; set; }
    public Guid? FamilyId { get; set; }
    public List<Link>? Links { get; set; }
}