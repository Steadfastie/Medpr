using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedprModels.Links;

namespace MedprModels.Interfaces;

public interface IHateoas
{
    public Guid Id { get; set; }
    public List<Link> Links { get; set; }
}
