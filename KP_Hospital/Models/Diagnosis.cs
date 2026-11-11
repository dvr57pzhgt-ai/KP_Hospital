using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_Hospital.Models
{
   
        public class Diagnosis
        {
            public int DiagnosisId { get; set; }

            public string Code { get; set; } = string.Empty;

            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public List<MedicalRecord> MedicalRecords { get; set; } = new();
        }
    
}
