using MedprDB.Entities;
using MedprModels.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace MedprModels.Requests;

public class PrescriptionModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, it should have the beginning date!")]
    [Column(TypeName = "DateTime2"), DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Cmon, it should have the ending date!")]
    [Column(TypeName = "DateTime2"), DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Cmon, it should've some dosage!")]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(1, int.MaxValue, ErrorMessage = "Input something greater than 0")]
    public int Dose { get; set; }

    [Required(ErrorMessage = "Someone is a patien here, isn't he?")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Some is a doctor here, isn't he?")]
    public Guid DoctorId { get; set; }

    [Required(ErrorMessage = "What will the patien take?")]
    public Guid DrugId { get; set; }
