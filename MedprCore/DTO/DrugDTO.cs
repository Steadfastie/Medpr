using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore.DTO
{
    public class DrugDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PharmGroup { get; set; }
        public int Price { get; set; }
    }
}
