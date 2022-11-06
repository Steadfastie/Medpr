using MedprDB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class FamilyMemberModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, is this user responsible for his family or not?")]
    public bool IsAdmin { get; set; }

    public List<string> Users { get; set; }

    [Required(ErrorMessage = "Someone is in this family, isn't he?")]
    public Guid UserId { get; set; }

    public UserModel User { get; set; }

    public List<string> Families { get; set; }

    [Required(ErrorMessage = "This user belongs to some family, isn't he?")]
    public Guid FamilyId { get; set; }

    public FamilyModel Family { get; set; }
}
