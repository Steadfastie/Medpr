using MedprDB.Entities;
using MedprModels.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Requests;

public class AppointmentModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, it should have happened sometime!")]
    [Column(TypeName = "DateTime2"), DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Cmon, it should've happend somewhere!")]
    [Column(TypeName = "decimal(18, 2)")]
    [StringLength(30, MinimumLength = 2)]
    public string Place { get; set; }

    [Required(ErrorMessage = "Someone took a shot, didn't he?")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Some doctor was assigned to it, wasn't he?")]
    public Guid DoctorId { get; set; }
}
