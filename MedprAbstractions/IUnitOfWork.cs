using MedprAbstractions.Repositories;
using MedprDB.Entities;

namespace MedprAbstractions;

public interface IUnitOfWork
{
    IRepository<Drug> Drugs { get; }
    IRepository<Family> Families { get; }
    IRepository<Vaccine> Vaccines { get; }
    IRepository<Doctor> Doctors { get; }

    Task<int> Commit();
}