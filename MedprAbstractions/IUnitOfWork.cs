using MedprAbstractions.Repositories;
using MedprDB.Entities;

namespace MedprAbstractions;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<Drug> Drugs { get; }
    IRepository<Family> Families { get; }
    IRepository<Vaccine> Vaccines { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<Vaccination> Vaccinations { get; }

    Task<int> Commit();
}