using MedprDB.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprMVC.Models
{
    public class VaccinationModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Cmon, it should have happened ever!")]
        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Cmon, it should protect at least for a day!")]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, int.MaxValue, ErrorMessage = "Input something greater than 0"), DataType(DataType.Duration)]
        public int DaysOfProtection { get; set; }

        [Required(ErrorMessage = "Someone took a shot, didn't he?")]
        public User User { get; set; }

        [Required(ErrorMessage = "Shot had a name, didn't it?")]
        public Vaccine Vaccine { get; set; }
    }
}
