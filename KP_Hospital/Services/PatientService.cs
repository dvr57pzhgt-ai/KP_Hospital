using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Services
{

        public class PatientService : IPatientService
        {
            private readonly AppDbContext _context;

            public PatientService(AppDbContext context)
            {
                _context = context;
            }

            public List<Patient> FindPatients(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    return _context.Patients.ToList();

                return _context.Patients.Where(p => p.FullName.Contains(name)).ToList();
            }

            public void AddPatient(Patient patient)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
            }
        }
    

}
