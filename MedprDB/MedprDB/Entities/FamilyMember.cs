using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB.Entities
{
    public class FamilyMember : IBaseEntity
    {
        public Guid Id { get; set; }

        public bool IsAdmin { get; set; }

        public Guid FamilyId { get; set; }
        public Family Family { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
