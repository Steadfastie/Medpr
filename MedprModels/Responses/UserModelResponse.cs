using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedprCore;
using MedprModels.Interfaces;
using MedprModels.Links;

namespace MedprModels.Responses;

public class UserModelResponse: IHateoas
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Role { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public List<Link>? Links { get; set; }
}
