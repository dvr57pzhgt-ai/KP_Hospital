using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace KP_Hospital.Data
{

        public class AppDbContext : DbContext
        {
            public DbSet<Patient> Patients { get; set; }
            public DbSet<Doctor> Doctors { get; set; }
            public DbSet<MedicalRecord> MedicalRecords { get; set; }
            public DbSet<Diagnosis> Diagnoses { get; set; }
            public DbSet<Prescription> Prescriptions { get; set; }
            public DbSet<Notification> Notifications { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=medical.db");
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<MedicalRecord>().HasKey(r => r.RecordId);
            }
    }
    

}
