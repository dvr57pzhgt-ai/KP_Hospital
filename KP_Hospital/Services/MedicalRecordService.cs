using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Services
{
    
        public class MedicalRecordService : IMedicalRecordService
        {
            private readonly AppDbContext _context;

            public MedicalRecordService(AppDbContext context)
            {
                _context = context;
            }

            public List<MedicalRecord> GetRecordsByPatientId(int patientId)
            {
                return _context.MedicalRecords
                               .Where(r => r.PatientId == patientId)
                               .ToList();
            }

            public void SaveRecord(MedicalRecord record)
            {
                if (record.DiagnosisId == null || record.DiagnosisId == 0)
                    throw new ArgumentException("Не выбран диагноз!");

                _context.MedicalRecords.Add(record);
                _context.SaveChanges();
            }
        }
    
}
