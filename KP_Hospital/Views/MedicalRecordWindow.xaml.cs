using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KP_Hospital.Data;
using KP_Hospital.Models;
using KP_Hospital.Services;


namespace KP_Hospital.Views
{
    /// <summary>
    /// Логика взаимодействия для окна введения и просмотра истории болезни.
    /// Реализует паттерн Memento для функции отмены действий.
    /// </summary>
    public partial class MedicalRecordWindow : Window
    {
        private readonly Patient _currentPatient;
        private readonly Doctor _currentDoctor;
        private readonly MedicalRecord _currentRecord;
        private readonly MedicalRecordCaretaker _caretaker;
        private bool _isRestoring = false;
        private bool _showAllHistory = false;

        /// <summary>
        /// Инициализирует окно для конкретного пациента и врача.
        /// </summary>

        public MedicalRecordWindow(Patient patient, Doctor doctor)
        {
            InitializeComponent();
            _currentPatient = patient;
            _currentDoctor = doctor;
            _currentRecord = new MedicalRecord();
            _caretaker = new MedicalRecordCaretaker();

            txtPatientName.Text = $"Пациент: {_currentPatient.FullName} (Врач: {_currentDoctor.FullName})";

            LoadDiagnoses();
            LoadHistory();
        }

        /// <summary>
        /// Загружает список всех диагнозов из базы данных в выпадающий список.
        /// </summary>
        private void LoadDiagnoses()
        {
            using var context = new AppDbContext();
            var diagnoses = context.Diagnoses.ToList();
            cmbDiagnosis.ItemsSource = diagnoses;
        }

        /// <summary>
        /// Загружает историю болезней текущего пациента из БД и выводит её в таблицу.
        /// Фильтрует записи по текущему врачу, если не нажата кнопка "Показать всю историю".
        /// </summary>

        private void LoadHistory()
        {
            using var context = new AppDbContext();

            var records = context.MedicalRecords
                .Where(r => r.PatientId == _currentPatient.PatientId)
                .Where(r => _showAllHistory || r.DoctorId == _currentDoctor.DoctorId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();

            dataGridHistory.Items.Clear();
            foreach (var r in records)
            {
                var diagName = context.Diagnoses.Find(r.DiagnosisId)?.Name ?? "Не указан";
                var docName = context.Doctors.Find(r.DoctorId)?.FullName ?? "Неизвестно";

                dataGridHistory.Items.Add(new
                {
                    VisitDate = r.VisitDate.ToString("dd.MM.yyyy"),
                    DoctorName = docName,
                    DiagnosisName = diagName,
                    Complaints = r.Complaints
                });
            }
        }

        /// <summary>
        /// Обработчик кнопки "Показать всю историю". Сбрасывает фильтр по врачу.
        /// </summary>
        private void BtnShowAllHistory_Click(object sender, RoutedEventArgs e)
        {
            _showAllHistory = true;
            LoadHistory();
            MessageBox.Show("Отображается полная история болезней.", "Информация");
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
        /// Обработчик нажатия на кнопку "Отменить". Восстанавливает предыдущее состояние формы.
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
        /// Обработчик нажатия на кнопку "Сохранить". 
        /// Сохраняет историю болезни, рецепт (если введен) и создает уведомление.
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDiagnosis.SelectedItem == null)
            {
                MessageBox.Show("Выберите диагноз!");
                return;
            }

            try
            {
                using var context = new AppDbContext();

                _currentRecord.PatientId = _currentPatient.PatientId;
                _currentRecord.DoctorId = _currentDoctor.DoctorId;
                _currentRecord.DiagnosisId = ((Diagnosis)cmbDiagnosis.SelectedItem).DiagnosisId;
                _currentRecord.VisitDate = DateTime.Now;
                _currentRecord.Complaints = txtComplaints.Text ?? "";
                _currentRecord.ObjectiveStatus = txtObjective.Text ?? "";
                _currentRecord.TreatmentPlan = txtTreatment.Text ?? "";

                context.MedicalRecords.Add(_currentRecord);
                context.SaveChanges();

                if (!string.IsNullOrWhiteSpace(txtMedication.Text))
                {
                    var newPrescription = new Prescription
                    {
                        RecordId = _currentRecord.RecordId,
                        Medication = txtMedication.Text,
                        Dosage = "По назначению",
                        Duration = "7 дней"
                    };
                    context.Prescriptions.Add(newPrescription);
                    context.SaveChanges();
                }

                MessageBox.Show("История болезни сохранена!", "Успех");

                txtComplaints.Text = "";
                txtObjective.Text = "";
                txtTreatment.Text = "";
                txtMedication.Text = "";
                cmbDiagnosis.SelectedItem = null;
                _currentRecord.RecordId = 0;

                LoadHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n{ex.InnerException?.Message}");
            }
        }

    }
}