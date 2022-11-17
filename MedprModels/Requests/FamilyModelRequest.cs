using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class FamilyModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    [StringLength(15, MinimumLength = 2)]
    public string Surname { get; set; }
}
