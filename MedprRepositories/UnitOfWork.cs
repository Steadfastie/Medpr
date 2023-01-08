using MedprAbstractions;
using MedprAbstractions.Repositories;
using MedprDB;
using MedprDB.Entities;

namespace MedprDataRepositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MedprDBContext _database;

        public IRepository<Drug> Drugs { get; }
        public IRepository<Family> Families { get; }
        public IRepository<Vaccine> Vaccines { get; }
        public IRepository<Vaccination> Vaccinations { get; }
        public IRepository<Doctor> Doctors { get; }
        public IRepository<User> Users { get; }
        public IRepository<Appointment> Appointments { get; }
        public IRepository<FamilyMember> FamilyMembers { get; }
        public IRepository<Prescription> Prescriptions { get; }

        public UnitOfWork(MedprDBContext database,
            IRepository<Drug> drugRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Family> familyRepository,
            IRepository<Vaccine> vaccineRepository,
            IRepository<Vaccination> vaccinations,
            IRepository<User> users,
            IRepository<Appointment> appointments,
            IRepository<FamilyMember> familyMembers,
            IRepository<Prescription> presctiptions)
        {
            _database = database;
            Drugs = drugRepository;
            Families = familyRepository;
            Vaccines = vaccineRepository;
            Doctors = doctorRepository;
            Vaccinations = vaccinations;
            Users = users;
            Appointments = appointments;
            FamilyMembers = familyMembers;
            Prescriptions = presctiptions;
        }

        public async Task<int> Commit()
        {
            return await _database.SaveChangesAsync();
        }
    }
}