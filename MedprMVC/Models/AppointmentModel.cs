using MedprDB.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprMVC.Models;

public class AppointmentModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, it should have happened sometime!")]
    [Column(TypeName = "DateTime2")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Cmon, it should've happend somewhere!")]
    [Column(TypeName = "decimal(18, 2)")]
    [StringLength(30, MinimumLength = 2)]
    public string Place { get; set; }

    public SelectList Users { get; set; }

    [Required(ErrorMessage = "Someone took a shot, didn't he?")]
    public Guid UserId { get; set; }

    public UserModel User { get; set; }

    public SelectList Doctors { get; set; }

    [Required(ErrorMessage = "Some doctor was assigned to it, wasn't he?")]
    public Guid DoctorId { get; set; }

    public DoctorModel Doctor { get; set; }
}
