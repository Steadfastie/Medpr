using MedprAbstractions;
using MedprAbstractions.Repositories;
using MedprDB;
using MedprDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public UnitOfWork(MedprDBContext database,
            IRepository<Drug> drugRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Family> familyRepository,
            IRepository<Vaccine> vaccineRepository,
            IRepository<Vaccination> vaccinations)
        {
            _database = database;
            Drugs = drugRepository;
            Families = familyRepository;
            Vaccines = vaccineRepository;
            Doctors = doctorRepository;
            Vaccinations = vaccinations;
        }

        public async Task<int> Commit()
        {
            return await _database.SaveChangesAsync();
        }
    }
}
