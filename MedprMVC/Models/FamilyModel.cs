using System.ComponentModel.DataAnnotations;

namespace MedprMVC.Models;

public class FamilyModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(15, MinimumLength = 2)]
    public string Surname { get; set; }

    public Guid Creator { get; set; }

    public List<FamilyMemberModel> Members { get; set; }
}