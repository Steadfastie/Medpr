using MedprAbstractions.Repositories;
using MedprDB.Entities;

namespace MedprAbstractions;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Drug> Drugs { get; }
    IRepository<Family> Families { get; }
    IRepository<Vaccine> Vaccines { get; }
    IRepository<Doctor> Doctors { get; }
    IRepository<Vaccination> Vaccinations { get; }
    IRepository<Appointment> Appointments { get; }
    IRepository<FamilyMember> FamilyMembers { get; }
    IRepository<Prescription> Prescriptions { get; }

    Task<int> Commit();
}