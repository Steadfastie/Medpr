using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprMVC.Models
{
    public class UserCredentialsModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Login { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(20, MinimumLength = 1)]
        public string Surname { get; set; }


        [Column(TypeName = "DateTime2")]
        public DateTime DateOfBirth { get; set; }
    }
}
