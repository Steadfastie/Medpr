using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore.DTO
{
    public class DoctorDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }
    }
}
