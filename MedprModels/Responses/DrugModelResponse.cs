using MedprModels.Interfaces;
using MedprModels.Links;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class DrugModelResponse: IHateoas
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PharmGroup { get; set; }
    public int Price { get; set; }
    public List<Link>? Links { get; set; }
}
