using MedprDB.Entities;
using MedprModels.Interfaces;
using MedprModels.Links;
using MedprModels.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedprModels.Responses;

public class FeedModelResponse
{
    public List<AppointmentModelResponse>? Appointments { get; set; }
    public List<VaccinationModelResponse>? Vaccintions { get; set; }
    public List<PrescriptionModelResponse>? Prescriptions { get; set; }
}
