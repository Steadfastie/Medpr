using MedprDB.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace MedprMVC.Models;

public class PrescriptionModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, it should have the beginning date!")]
    [Column(TypeName = "DateTime2"), DataType(DataType.Date)]
    [Remote("CheckDate", "Prescriptions",
        HttpMethod = WebRequestMethods.Http.Post, 
        ErrorMessage = "Starting date should be before ending",
        AdditionalFields = "EndDate")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Cmon, it should have the ending date!")]
    [Column(TypeName = "DateTime2"), DataType(DataType.Date)]
    [Remote("CheckDate", "Prescriptions",
        HttpMethod = WebRequestMethods.Http.Post,
        ErrorMessage = "Starting date should be before ending",
        AdditionalFields = "StartDate")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Cmon, it should've some dosage!")]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(1, int.MaxValue, ErrorMessage = "Input something greater than 0")]
    public int Dose { get; set; }

    public SelectList Users { get; set; }

    [Required(ErrorMessage = "Someone is a patien here, isn't he?")]
    public Guid UserId { get; set; }

    public UserModel User { get; set; }

    public SelectList Doctors { get; set; }

    [Required(ErrorMessage = "Some is a doctor here, isn't he?")]
    public Guid DoctorId { get; set; }

    public DoctorModel Doctor { get; set; }

    public SelectList Drugs { get; set; }

    [Required(ErrorMessage = "What will the patien take?")]
    public Guid DrugId { get; set; }

    public DrugModel Drug { get; set; }
}
