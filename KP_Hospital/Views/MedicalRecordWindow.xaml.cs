using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KP_Hospital.Data;
using KP_Hospital.Models;
using KP_Hospital.Services;

namespace KP_Hospital.Views
{
    /// <summary>
    /// Логика взаимодействия для окна введения истории болезни.
    /// Реализует паттерн Memento для функции отмены действий.
    /// </summary>
    public partial class MedicalRecordWindow : Window
    {
        private readonly Patient _currentPatient;
        private readonly MedicalRecord _currentRecord;
        private readonly MedicalRecordCaretaker _caretaker;
        private bool _isRestoring = false;

        /// <summary>
        /// Инициализирует окно для конкретного пациента.
        /// </summary>
       
        public MedicalRecordWindow(Patient patient)
        {
            InitializeComponent();
            _currentPatient = patient;
            _currentRecord = new MedicalRecord();
            _caretaker = new MedicalRecordCaretaker();

            txtPatientName.Text = $"Пациент: {_currentPatient.FullName}";

            LoadDiagnoses();
        }

        /// <summary>
        /// Загружает список всех диагнозов из базы данных в выпадающий список (ComboBox).
        /// </summary>
        private void LoadDiagnoses()
        {
            using var context = new AppDbContext();
            var diagnoses = context.Diagnoses.ToList();
            cmbDiagnosis.ItemsSource = diagnoses;
        }

        /// <summary>
        /// Обработчик изменения текста в полях ввода. Создает "снимок" (Memento) для отмены.
        /// </summary>
       
        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isRestoring) return;
            _currentRecord.Complaints = txtComplaints.Text;
            _currentRecord.ObjectiveStatus = txtObjective.Text;
            _currentRecord.TreatmentPlan = txtTreatment.Text;
            _caretaker.Save(_currentRecord);
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Отменить". Восстанавливает предыдущее состояние (Memento).
        /// </summary>
        private void BtnUndo_Click(object sender, RoutedEventArgs e)
        {
            _isRestoring = true; 

            if (_caretaker.Undo(_currentRecord))
            {
                txtComplaints.Text = _currentRecord.Complaints;
                txtObjective.Text = _currentRecord.ObjectiveStatus;
                txtTreatment.Text = _currentRecord.TreatmentPlan;
            }

            _isRestoring = false;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Сохранить". Сохраняет историю болезни в базу данных.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDiagnosis.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите диагноз из списка!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var context = new AppDbContext();
                _currentRecord.PatientId = _currentPatient.PatientId;
                _currentRecord.DoctorId = 1; 
                _currentRecord.DiagnosisId = ((Diagnosis)cmbDiagnosis.SelectedItem).DiagnosisId;
                _currentRecord.VisitDate = DateTime.Now;
                _currentRecord.Complaints = txtComplaints.Text;
                _currentRecord.ObjectiveStatus = txtObjective.Text;
                _currentRecord.TreatmentPlan = txtTreatment.Text;

                context.MedicalRecords.Add(_currentRecord);
                context.SaveChanges();

                MessageBox.Show("История болезни успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}