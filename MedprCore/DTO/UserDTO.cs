using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public string? FullName { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? DateOfBirth { get; set; }
    }
}
