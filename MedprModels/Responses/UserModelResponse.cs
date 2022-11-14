using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedprCore;

namespace MedprModels.Responses;

public class UserModelResponse
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Role { get; set; }
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
