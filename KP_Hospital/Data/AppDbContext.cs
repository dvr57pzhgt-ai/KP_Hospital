using KP_Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace KP_Hospital.Data
{
    /// <summary>
    /// Контекст базы данных 
    /// Управляет всеми таблицами приложения.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        /// <summary>
        /// Настройка параметров подключения к серверу БД.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\NameServer;Database=MedicalDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        /// <summary>
        /// Настройка моделей, явное указание ключей.
        /// </summary>

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MedicalRecord>().HasKey(r => r.RecordId);
        }
    }
}