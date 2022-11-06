using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedprCore;

namespace MedprModels.Requests;

public class UserModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Provide some credentials to login")]
    [StringLength(30, MinimumLength = 5)]
    [EmailAddress]
    public string Login { get; set; }

    [Required(ErrorMessage = "Provide some password for security")]
    [StringLength(30, MinimumLength = 5)]
    public string Password { get; set; }

    [StringLength(30, MinimumLength = 1)]
    public string? FullName { get; set; }

    [Column(TypeName = "DateTime2")]
    [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
    public DateTime? DateOfBirth { get; set; }

    public int? SelectedRole { get; set; }

    public List<string> Roles { get; set; }
}
