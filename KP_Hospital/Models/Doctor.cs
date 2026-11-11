using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KP_Hospital.Models
{

        public class Doctor
        {
            public int DoctorId { get; set; }

          
            public string FullName { get; set; } = string.Empty;

            public string Specialization { get; set; } = string.Empty;

            public string Cabinet { get; set; } = string.Empty;

            public string Phone { get; set; } = string.Empty;

            public string Login { get; set; } = string.Empty;

            public string PasswordHash { get; set; } = string.Empty;

            public List<MedicalRecord> MedicalRecords { get; set; } = new();
            public List<Notification> Notifications { get; set; } = new();
       }
    

}
