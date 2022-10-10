using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprMVC.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Provide some credentials to login")]
        [StringLength(30, MinimumLength = 5)]
        [EmailAddress]
        public string Login { get; set; }

        [Required(ErrorMessage = "Provide some password for security")]
        [StringLength(30, MinimumLength = 5)]
        public string PasswordHash { get; set; }

        [StringLength(30, MinimumLength = 1)]
        public string? FullName { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? DateOfBirth { get; set; }
    }
}
