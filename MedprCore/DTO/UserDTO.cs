using System.ComponentModel.DataAnnotations.Schema;

namespace MedprCore.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string? FullName { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? DateOfBirth { get; set; }
    }
}