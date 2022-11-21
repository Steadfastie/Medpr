using MedprModels.Interfaces;
using MedprModels.Links;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Responses;

public class RandomDrugModel
{
    public string Name { get; set; }
    public string PharmGroup { get; set; }
}
