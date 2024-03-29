﻿using System.ComponentModel.DataAnnotations;

namespace MedprModels.Requests;

public class AppointmentModelRequest
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Cmon, it should have happened sometime!")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Cmon, it should've happend somewhere!")]
    [StringLength(30, MinimumLength = 2)]
    public string Place { get; set; }

    [Required(ErrorMessage = "Someone took a shot, didn't he?")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Some doctor was assigned to it, wasn't he?")]
    public Guid DoctorId { get; set; }
}