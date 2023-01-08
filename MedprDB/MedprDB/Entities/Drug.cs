namespace MedprDB.Entities
{
    public class Drug : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PharmGroup { get; set; }

        public int Price { get; set; }

        public List<Prescription> Prescriptions { get; set; }
    }
}