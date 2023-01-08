namespace MedprDB.Entities
{
    public class Doctor : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Experience { get; set; }

        public List<Prescription> Prescriptions { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
}