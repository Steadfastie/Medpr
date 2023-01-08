namespace MedprDB.Entities
{
    public class FamilyMember : IBaseEntity
    {
        public Guid Id { get; set; }

        public bool IsAdmin { get; set; }

        public Guid FamilyId { get; set; }
        public Family Family { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}