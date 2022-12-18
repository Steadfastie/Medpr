using MedprDB.Entities;
using MedprModels.Interfaces;
using MedprModels.Links;
using MedprModels.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Responses;

public class FamilyMemberModelResponse: IHateoas
{
    public Guid Id { get; set; }
    public bool IsAdmin { get; set; }
    public UserModelResponse User { get; set; }
    public Guid? FamilyId { get; set; }
    public List<Link>? Links { get; set; }
}
