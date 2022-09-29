using MedprAbstractions.Repositories;
using MedprDB.Entities;

namespace MedprAbstractions;

public interface IUnitOfWork
{
    IRepository<Drug> Drugs { get; }

    Task<int> Commit();
}