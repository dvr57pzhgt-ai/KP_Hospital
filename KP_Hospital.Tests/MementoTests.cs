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
            // 1. Arrange (Подготовка)
            var record = new MedicalRecord
            {
                Complaints = "Исходные жалобы",
                TreatmentPlan = "Лечение 1"
            };

            // 2. Act (Действие)
            var memento = record.SaveState(); // Делаем снимок

            record.Complaints = "Новые жалобы"; // Меняем данные
            record.TreatmentPlan = "Лечение 2";

            record.RestoreState(memento); // Восстанавливаем

            // 3. Assert (Проверка)
            Assert.That(record.Complaints, Is.EqualTo("Исходные жалобы"));
            Assert.That(record.TreatmentPlan, Is.EqualTo("Лечение 1"));
        }
    }
}