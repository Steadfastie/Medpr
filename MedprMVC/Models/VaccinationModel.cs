﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprMVC.Models
{
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

        public SelectList Users { get; set; }

        [Required(ErrorMessage = "Someone took a shot, didn't he?")]
        public Guid UserId { get; set; }

        public UserModel User { get; set; }

        public SelectList Vaccines { get; set; }

        [Required(ErrorMessage = "Shot had a name, didn't it?")]
        public Guid VaccineId { get; set; }

        public VaccineModel Vaccine { get; set; }
    }
}