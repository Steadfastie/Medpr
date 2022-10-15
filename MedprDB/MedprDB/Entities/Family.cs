using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB.Entities
{
    public class Family : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Surname { get; set; }

        public Guid Creator { get; set; }

        public List<FamilyMember> FamilyMember { get; set; }
    }
}
