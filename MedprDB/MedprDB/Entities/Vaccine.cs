namespace MedprDB.Entities
{
    public class Vaccine : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Reason { get; set; }

        public int Price { get; set; }

        public List<Vaccination> Vaccinations { get; set; }
    }
}