using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;


namespace KP_Hospital.Models
{
       
        public class Patient
        {
            public int PatientId { get; set; }

            public string FullName { get; set; } = string.Empty;

            public DateTime BirthDate { get; set; }

          
            public string Gender { get; set; } = string.Empty;

            public string InsuranceNumber { get; set; } = string.Empty;

            public string Address { get; set; } = string.Empty;

            public string Phone { get; set; } = string.Empty;

   
            public List<MedicalRecord> MedicalRecords { get; set; } = new();
        }
    


}
