using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB.Entities
{
    public class Vaccine : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Reason { get; set; }

        public int Price { get; set; }

        public List<Vaccination> Vaccinations { get; set; }
    }
}
