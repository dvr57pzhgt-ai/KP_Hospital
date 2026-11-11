using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_Hospital.Models
{
  
        public class Prescription
        {
            public int PrescriptionId { get; set; }

            public string Medication { get; set; } = string.Empty;

            public string Dosage { get; set; } = string.Empty;

            public string Duration { get; set; } = string.Empty;

            public int RecordId { get; set; }
            public MedicalRecord? MedicalRecord { get; set; }
        }
    
}
