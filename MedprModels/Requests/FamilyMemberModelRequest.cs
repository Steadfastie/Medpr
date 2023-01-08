using System.ComponentModel.DataAnnotations;

namespace MedprModels.Requests;

public class FamilyMemberModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, is this user responsible for his family or not?")]
    public bool IsAdmin { get; set; }

    [Required(ErrorMessage = "Someone is in this family, isn't he?")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "This user belongs to some family, isn't he?")]
    public Guid FamilyId { get; set; }
}