using MedprModels.Interfaces;
using MedprModels.Links;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Responses;

public class VaccineModelResponse: IHateoas
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string Reason { get; set; }

    public int Price { get; set; }
    public List<Link>? Links { get; set; }
}
