using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB.Entities
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
