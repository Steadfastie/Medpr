using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprMVC.Models;

public class VaccineModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "It couldn't be nameless")]
    [StringLength(15, MinimumLength = 3)]
    public string Name { get; set; }

    [DisplayName("Infectious disease")]
    [StringLength(50, MinimumLength = 3)]
    [Required(ErrorMessage = "Drug should belong to some pharmaceutical group")]
    public string Reason { get; set; }

    [Required(ErrorMessage = "Cmon, it should cost something")]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, int.MaxValue, ErrorMessage = "Input something greater than 0"), DataType(DataType.Currency)]
    public int Price { get; set; }
}