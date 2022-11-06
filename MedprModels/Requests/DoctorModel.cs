using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class DoctorModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [StringLength(15, MinimumLength = 3)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Cmon, diploma counts too")]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 100, ErrorMessage = "Experience can't be negative or greater than 100"), DataType(DataType.Duration)]
    public int Experience { get; set; }
}
