using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class UserModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Provide some credentials to login")]
    [StringLength(30, MinimumLength = 5)]
    [EmailAddress]
    public string Login { get; set; }

    [Required(ErrorMessage = "Provide some password for security")]
    [StringLength(30, MinimumLength = 5)]
    public string Password { get; set; }

    [StringLength(30, MinimumLength = 5)]
    public string? NewPassword { get; set; }

    public string? Role { get; set; }

    [StringLength(30, MinimumLength = 1)]
    public string? FullName { get; set; }

    [Column(TypeName = "DateTime2")]
    [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
    public string? DateOfBirth { get; set; }
}