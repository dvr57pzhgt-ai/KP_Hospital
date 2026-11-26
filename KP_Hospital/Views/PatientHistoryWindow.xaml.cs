using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Views
{
    /// <summary>
    /// Логика взаимодействия для PatientHistoryWindow.xaml
    /// </summary>
    public partial class PatientHistoryWindow : Window
    {
        private readonly Patient _currentPatient;

        public PatientHistoryWindow(Patient patient)
        {
            InitializeComponent();
            _currentPatient = patient;
            txtGreeting.Text = $"Здравствуйте, {_currentPatient.FullName}! Ваша история болезней:";
            LoadHistory();
        }

        private void LoadHistory()
        {
            using var context = new AppDbContext();

            var records = context.MedicalRecords
                .Where(r => r.PatientId == _currentPatient.PatientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            dataGridMyHistory.Items.Clear();
            foreach (var r in records)
            {
                var diagName = context.Diagnoses.Find(r.DiagnosisId)?.Name ?? "Не указан";
                var docName = context.Doctors.Find(r.DoctorId)?.FullName ?? "Неизвестно";

                dataGridMyHistory.Items.Add(new
                {
                    VisitDate = r.VisitDate.ToString("dd.MM.yyyy"),
                    DoctorName = docName,
                    DiagnosisName = diagName,
                    TreatmentPlan = r.TreatmentPlan
                });
            }
        }
    }
}


