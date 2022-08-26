using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB
{
    public class Family
    {
        public Guid Id { get; set; }

        public string Surname { get; set; }

        public List<FamilyMember> FamilyMember { get; set; }
    }
}
