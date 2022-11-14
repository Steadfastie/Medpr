using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedprCore;

namespace MedprModels.Requests;

public class UserModelRequest
{

    [Required(ErrorMessage = "Provide some credentials to login")]
    [StringLength(30, MinimumLength = 5)]
    [EmailAddress]
    public string Login { get; set; }

    [Required(ErrorMessage = "Provide some password for security")]
    [StringLength(30, MinimumLength = 5)]
    public string Password { get; set; }
}
