using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KP_Hospital.Models
{
    
        public class Notification
        {
            public int NotificationId { get; set; }

            public string Type { get; set; } = string.Empty;

            public string Message { get; set; } = string.Empty;

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public bool IsRead { get; set; }

            public int DoctorId { get; set; }
            public Doctor? Doctor { get; set; }
        }
    
}
