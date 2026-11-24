using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Services
{
    /// <summary>
    /// Сервис для управления пациентами (поиск, добавление).
    /// </summary>

    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;

        public PatientService(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Ищет пациентов по ФИО.
        /// </summary>

        public List<Patient> FindPatients(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _context.Patients.ToList();

            return _context.Patients.Where(p => p.FullName.Contains(name)).ToList();
        }

        /// <summary>
        /// Добавляет нового пациента в базу данных.
        /// </summary>
        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

    }
    

}
