using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB
{
    internal class MedprDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        private const string ConnectionString =
            "Server=LKUMBRELLA;" +
            "Database=ITA-Medpr;" +
            "Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
