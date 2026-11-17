using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Models;

namespace KP_Hospital.Services
{
    /// <summary>
    /// Интерфейс для сервиса работы с историями болезней.
    /// Определяет контракт для получения и сохранения медицинских записей.
    /// </summary>
    public interface IMedicalRecordService
    {
        List<MedicalRecord> GetRecordsByPatientId(int patientId);
        void SaveRecord(MedicalRecord record);
    }
}
