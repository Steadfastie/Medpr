using MedprModels.Interfaces;
using MedprModels.Links;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Responses;

public class FamilyModelResponse: IHateoas
{
    public Guid Id { get; set; }
    public string Surname { get; set; }
    public Guid Creator { get; set; }
    public List<FamilyMemberModelResponse> Members { get; set; }
    public List<Link>? Links { get; set; }
}
