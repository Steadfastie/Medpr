using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedprDB
{
    public class MedprDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }

        public MedprDBContext(DbContextOptions<MedprDBContext> options)
        : base(options)
        {
        }
    }
}
