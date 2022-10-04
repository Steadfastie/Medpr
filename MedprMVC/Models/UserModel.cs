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
        public string Login { get; set; }

        [Required(ErrorMessage = "Provide some password for security")]
        [StringLength(30, MinimumLength = 5)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 1)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Some general information")]
        [Column(TypeName = "DateTime2")]
        public DateTime DateOfBirth { get; set; }
    }
}
