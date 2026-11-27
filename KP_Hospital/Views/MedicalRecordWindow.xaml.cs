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

                var prescription = context.Prescriptions.FirstOrDefault(p => p.RecordId == r.RecordId);
                bool isExpired = false;
                if (prescription != null && (DateTime.Now - r.VisitDate).Days > 30)
                {
                    isExpired = true;
                }

                string sickLeaveStatus = "Нет";
                if (r.IsSickLeave)
                {
                    if (r.SickLeaveEndDate != null)
                    {
                        sickLeaveStatus = "Снят с больничного";
                    }
                    else
                    {
                        int totalDays = 7 + r.SickLeaveExtensionDays;
                        int daysPassed = (DateTime.Now - r.VisitDate).Days;
                        int remainingDays = totalDays - daysPassed;

                        if (remainingDays > 0)
                            sickLeaveStatus = $"Осталось {remainingDays} дн.";
                        else
                            sickLeaveStatus = "Закончился срок больничного";
                    }
                }

                dataGridHistory.Items.Add(new
                {
                    RecordId = r.RecordId, 
                    VisitDate = r.VisitDate.ToString("dd.MM.yyyy"),
                    DoctorName = docName,
                    DiagnosisName = diagName,
                    Complaints = r.Complaints,
                    SickLeaveStatus = sickLeaveStatus, 
                    IsExpired = isExpired
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
            if (cmbDiagnosis.SelectedItem == null && string.IsNullOrWhiteSpace(cmbDiagnosis.Text))
            {
                MessageBox.Show("Выберите диагноз из списка или напишите свой!");
                return;
            }

            try
            {
                using var context = new AppDbContext();

                int? finalDiagnosisId = null;

                if (cmbDiagnosis.SelectedItem is Diagnosis selectedDiagnosis)
                {
                    finalDiagnosisId = selectedDiagnosis.DiagnosisId;
                }
                else if (!string.IsNullOrWhiteSpace(cmbDiagnosis.Text))
                {
                    string manualText = cmbDiagnosis.Text.Trim();

                    var existingDiag = context.Diagnoses.FirstOrDefault(d => d.Name == manualText);

                    if (existingDiag != null)
                    {
                        finalDiagnosisId = existingDiag.DiagnosisId;
                    }
                    else
                    {
                        var newDiag = new Diagnosis
                        {
                            Code = "MANUAL", 
                            Name = manualText,
                            Description = "Введено врачом вручную"
                        };
                        context.Diagnoses.Add(newDiag);
                        context.SaveChanges(); 

                        finalDiagnosisId = newDiag.DiagnosisId;
                    }
                }

                _currentRecord.PatientId = _currentPatient.PatientId;
                _currentRecord.DoctorId = _currentDoctor.DoctorId;
                _currentRecord.DiagnosisId = finalDiagnosisId; 
                _currentRecord.VisitDate = DateTime.Now;
                _currentRecord.Complaints = txtComplaints.Text ?? "";
                _currentRecord.ObjectiveStatus = txtObjective.Text ?? "";
                _currentRecord.TreatmentPlan = txtTreatment.Text ?? "";
                _currentRecord.IsSickLeave = chkSickLeave.IsChecked ?? false;

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

                    ExportPrescriptionToFile(_currentPatient.FullName, cmbDiagnosis.Text, txtMedication.Text);
                }

                var notification = new Notification
                {
                    DoctorId = _currentDoctor.DoctorId,
                    Message = $"Создана история болезни для пациента {_currentPatient.FullName}",
                    Type = "Медицинское",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                context.Notifications.Add(notification);
                context.SaveChanges();

                MessageBox.Show("История болезни сохранена!", "Успех");

                txtComplaints.Text = "";
                txtObjective.Text = "";
                txtTreatment.Text = "";
                txtMedication.Text = "";
                chkSickLeave.IsChecked = false;
                cmbDiagnosis.SelectedItem = null;
                cmbDiagnosis.Text = "";
                _currentRecord.RecordId = 0;

                LoadHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n{ex.InnerException?.Message}");
            }
        }
        /// <summary>
        /// Открывает диалог сохранения файла и записывает в него рецепт.
        /// </summary>

        private void ExportPrescriptionToFile(string patientName, string diagnosis, string medication)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

                saveFileDialog.Title = "Сохранить рецепт";
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FileName = $"Рецепт_{patientName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.txt";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (saveFileDialog.ShowDialog() == true)
                {
                    string fullPath = saveFileDialog.FileName;

                    string content = $"Рецепт врача\n" + $"Дата выдачи: {DateTime.Now:dd.MM.yyyy HH:mm}\n" + $"Врач: {_currentDoctor.FullName}\n" + $"Пациент: {patientName}\n" +
     $"Диагноз: {diagnosis}\n" + $"Назначение:\n" + $"{medication}\n" + $"Режим приема: По назначению врача\n" + $"Курс: 7 дней\n";

                    System.IO.File.WriteAllText(fullPath, content);

                    MessageBox.Show("Рецепт успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось сохранить рецепт: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Обработчик кнопки "Закрыть". Закрывает окно истории болезни.
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Обработчик двойного клика по истории болезней. Открывает окно управления больничным.
        /// </summary>
        private void DataGridHistory_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dynamic selected = dataGridHistory.SelectedItem;
            if (selected != null && selected.RecordId != null)
            {
                using var context = new AppDbContext();
                var record = context.MedicalRecords.Find((int)selected.RecordId);

                if (record != null && record.IsSickLeave) 
                {
                    var manageWindow = new ManageSickLeaveWindow(record);
                    if (manageWindow.ShowDialog() == true)
                    {
                        LoadHistory(); 
                    }
                }
            }
        }

    }
}