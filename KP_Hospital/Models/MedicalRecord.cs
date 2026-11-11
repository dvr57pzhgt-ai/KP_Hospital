using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace KP_Hospital.Models
{
        public class MedicalRecord
        {
            public int RecordId { get; set; }

            public DateTime VisitDate { get; set; } = DateTime.Now;

            public string Complaints { get; set; } = string.Empty;

            public string ObjectiveStatus { get; set; } = string.Empty;

            public string TreatmentPlan { get; set; } = string.Empty;

            public int PatientId { get; set; }
            public Patient? Patient { get; set; }

            public int DoctorId { get; set; }
            public Doctor? Doctor { get; set; }

            public int? DiagnosisId { get; set; }
            public Diagnosis? DiagnosisInfo { get; set; }

            public List<Prescription> Prescriptions { get; set; } = new();
        }
    
}
    

