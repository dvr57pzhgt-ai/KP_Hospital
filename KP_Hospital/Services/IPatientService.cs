using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Models;

namespace KP_Hospital.Services
{

    public interface IPatientService
    {
        List<Patient> FindPatients(string name);
        void AddPatient(Patient patient);
    }

}
