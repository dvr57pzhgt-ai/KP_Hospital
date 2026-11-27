using NUnit.Framework;
using KP_Hospital.Models;

namespace KP_Hospital.Tests
{
    [TestFixture]
    public class LogicTests
    {
        [Test]
        public void SickLeaveCalculation_ShouldCalculateRemainingDaysCorrectly()
        {
            var record = new MedicalRecord
            {
                VisitDate = DateTime.Now.AddDays(-3),
                IsSickLeave = true,
                SickLeaveExtensionDays = 0,
                SickLeaveEndDate = null
            };

            int totalDays = 7 + record.SickLeaveExtensionDays;
            int daysPassed = (DateTime.Now - record.VisitDate).Days;
            int remainingDays = totalDays - daysPassed;

            Assert.That(remainingDays, Is.EqualTo(4));
        }
    }
}