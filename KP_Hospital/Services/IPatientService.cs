using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Models;

namespace KP_Hospital.Services
{
    /// <summary>
    /// Интерфейс для сервиса управления пациентами.
    /// Определяет контракт для поиска и добавления пациентов.
    /// </summary>

    public interface IPatientService
    {
        List<Patient> FindPatients(string name);
        void AddPatient(Patient patient);
    }

}
