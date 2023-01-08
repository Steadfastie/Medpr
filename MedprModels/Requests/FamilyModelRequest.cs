using System.ComponentModel.DataAnnotations;

namespace MedprModels.Requests;

public class FamilyModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(15, MinimumLength = 2)]
    public string Surname { get; set; }
}