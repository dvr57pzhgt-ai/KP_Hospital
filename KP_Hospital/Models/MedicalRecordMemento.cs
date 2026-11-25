namespace KP_Hospital.Models
{
    public class MedicalRecordMemento
    {
        public string Complaints { get; }
        public string ObjectiveStatus { get; }
        public string TreatmentPlan { get; }

        public MedicalRecordMemento(string complaints, string objectiveStatus, string treatmentPlan)
        {
            Complaints = complaints;
            ObjectiveStatus = objectiveStatus;
            TreatmentPlan = treatmentPlan;
        }
    }
}