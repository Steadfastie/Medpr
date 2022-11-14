using MedprDB.Entities;
using MedprModels.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class VaccinationModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, it should have happened sometime!")]
    [Column(TypeName = "DateTime2")]
    [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Cmon, it should protect at least for a day!")]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, int.MaxValue, ErrorMessage = "Input something greater than 0"), DataType(DataType.Duration)]
    public int DaysOfProtection { get; set; }

    public List<string> Users { get; set; }

    [Required(ErrorMessage = "Someone took a shot, didn't he?")]
    public Guid UserId { get; set; }

    public UserModelResponse User { get; set; }

    public List<string> Vaccines { get; set; }

    [Required(ErrorMessage = "Shot had a name, didn't it?")]
    public Guid VaccineId { get; set; }

    public VaccineModelRequest Vaccine { get; set; }
}
