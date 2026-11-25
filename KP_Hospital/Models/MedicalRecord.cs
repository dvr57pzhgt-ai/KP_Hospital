using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KP_Hospital.Models
{
    public class MedicalRecord
    {
        public int RecordId { get; set; }

        public DateTime VisitDate { get; set; }

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

        public bool IsSickLeave { get; set; }


        /// <summary>
        /// Создает снимок текущего состояния истории болезни.
        /// </summary>
        public MedicalRecordMemento SaveState()
        {
            return new MedicalRecordMemento(Complaints, ObjectiveStatus, TreatmentPlan);
        }

        /// <summary>
        /// Восстанавливает состояние из снимка.
        /// </summary>
        public void RestoreState(MedicalRecordMemento memento)
        {
            Complaints = memento.Complaints;
            ObjectiveStatus = memento.ObjectiveStatus;
            TreatmentPlan = memento.TreatmentPlan;
        }
      
    }
}