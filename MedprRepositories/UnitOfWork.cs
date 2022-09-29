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

        public UnitOfWork(MedprDBContext database,
            IRepository<Drug> drugRepository)
        {
            _database = database;
            Drugs = drugRepository;
        }

        public async Task<int> Commit()
        {
            return await _database.SaveChangesAsync();
        }
    }
}
