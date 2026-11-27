using NUnit.Framework;
using KP_Hospital.Models;

namespace KP_Hospital.Tests
{
    [TestFixture]
    public class MementoTests
    {
        [Test]
        public void SaveAndRestoreState_ShouldRestorePreviousValues()
        {
            var record = new MedicalRecord
            {
                Complaints = "Исходные жалобы",
                TreatmentPlan = "Лечение 1"
            };

            var memento = record.SaveState();

            record.Complaints = "Новые жалобы"; 
            record.TreatmentPlan = "Лечение 2";

            record.RestoreState(memento);

            Assert.That(record.Complaints, Is.EqualTo("Исходные жалобы"));
            Assert.That(record.TreatmentPlan, Is.EqualTo("Лечение 1"));
        }
    }
}