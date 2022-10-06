using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MedprCore.DTO;

public class FamilyMemberDTO
{
    public Guid Id { get; set; }

    public bool IsAdmin { get; set; }

    public Guid UserId { get; set; }

    public Guid FamilyId { get; set; }
}
