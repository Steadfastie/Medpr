namespace MedprCore.DTO;

public class FamilyMemberDTO
{
    public Guid Id { get; set; }

    public bool IsAdmin { get; set; }

    public Guid UserId { get; set; }

    public Guid FamilyId { get; set; }
}