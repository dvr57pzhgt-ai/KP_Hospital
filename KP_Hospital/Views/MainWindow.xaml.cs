using System;
using System.Windows;
using System.Linq;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Views
{
    public partial class MainWindow : Window
    {
        private Doctor _currentDoctor; 

        public MainWindow(Doctor doctor)
        {
            InitializeComponent();
            _currentDoctor = doctor; 
            LoadData();
        }

        /// <summary>
        /// Загружает всех пациентов из БД в таблицу вручную.
        /// </summary>
        private void LoadData()
        {
            try
            {
                using var context = new AppDbContext();

                
                var patients = context.Patients.ToList();

                
                dataGridPatients.Items.Clear();

               
                if (patients.Count == 0)
                {
                    MessageBox.Show("В базе данных нет ни одного пациента!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

               
                foreach (var p in patients)
                {
                    dataGridPatients.Items.Add(new
                    {
                        Id = p.PatientId,
                        ФИО = p.FullName,
                        ДатаРождения = p.BirthDate.ToString("dd.MM.yyyy"),
                        Телефон = p.Phone
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}\n\n{ex.InnerException?.Message}",
                                "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Кнопка "Сбросить".
        /// </summary>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty; 
            LoadData(); 
        }

        /// <summary>
        /// Обработчик кнопки "Найти". Ищет пациентов по ФИО или номеру полиса.
        /// </summary>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            using var context = new AppDbContext();

            var patients = context.Patients
                .Where(p => p.FullName.Contains(searchText) || p.InsuranceNumber.Contains(searchText))
                .ToList();

            dataGridPatients.Items.Clear();

            foreach (var p in patients)
            {
                dataGridPatients.Items.Add(new
                {
                    Id = p.PatientId,
                    ФИО = p.FullName,
                    ДатаРождения = p.BirthDate.ToString("dd.MM.yyyy"),
                    Телефон = p.Phone
                });
            }
        }
        /// <summary>
        /// Кнопка "+ Добавить пациента".
        /// </summary>
        private void BtnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            var patientWindow = new PatientWindow();

            
            if (patientWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }
        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGridPatients.SelectedItem != null)
            {
                dynamic selected = dataGridPatients.SelectedItem;
                int patientId = selected.Id;

                using var context = new AppDbContext();
                var patient = context.Patients.Find(patientId);

                if (patient != null)
                {
                    var recordWindow = new MedicalRecordWindow(patient, _currentDoctor);
                    recordWindow.ShowDialog();
                }
            }
        }
           
            /// <summary>
            /// Обработчик нажатия на кнопку "Уведомления". 
            /// Загружает список уведомлений для текущего врача из базы данных.
            /// </summary>
           
         private void BtnNotifications_Click(object sender, RoutedEventArgs e)
         {
                using var context = new AppDbContext();

                var notifications = context.Notifications
                    .Where(n => n.DoctorId == 1)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToList();

                if (notifications.Count == 0)
                {
                    MessageBox.Show("У вас нет новых уведомлений.", "Уведомления");
                    return;
                }

                string message = "Ваши уведомления:\n\n";
                foreach (var n in notifications)
                {
                    message += $"[{n.CreatedAt:dd.MM.yyyy HH:mm}] {n.Message}\n";
                }

                MessageBox.Show(message, "Уведомления", MessageBoxButton.OK, MessageBoxImage.Information);
         }
        /// <summary>
        /// Обработчик пункта меню "Выход".
        /// </summary>
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Обработчик пункта меню "О программе".
        /// </summary>
        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Курсовой проект\n" +
                "Тема: Разработка информационной системы для ведения историй болезней\n" +
                "Выполнил: Табачинская Милана Владимировна\n" +
                "Группа: 3пк2\n" +
                "Технологии: C#, WPF, MS SQL Server, Entity Framework Core, MVVM, NUnit",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Формирует отчет по всем пациентам и сохраняет его в выбранный файл.
        /// </summary>
        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new AppDbContext();

                var patients = context.Patients.ToList();

                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Title = "Сохранить отчет";
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|CSV файлы (*.csv)|*.csv";
                saveFileDialog.FileName = $"Отчет_по_пациентам_{DateTime.Now:yyyyMMdd}.txt";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string content = "Отчет по пациентам клиники\n" + $"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}\n" + $"Всего пациентов в базе: {patients.Count}\n" ;

                    foreach (var p in patients)
                    {
                        int visitsCount = context.MedicalRecords.Count(r => r.PatientId == p.PatientId);

                        content += $"ID: {p.PatientId}\n";
                        content += $"ФИО: {p.FullName}\n";
                        content += $"Дата рождения: {p.BirthDate:dd.MM.yyyy}\n";
                        content += $"Полис: {p.InsuranceNumber}\n";
                        content += $"Телефон: {p.Phone}\n";
                        content += $"Количество приемов: {visitsCount}\n";

                    }

                    if (saveFileDialog.FileName.EndsWith(".csv"))
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, content, System.Text.Encoding.UTF8);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(saveFileDialog.FileName, content);
                    }

                    MessageBox.Show("Отчет успешно сформирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Удаляет выбранного пациента из базы данных.
        /// </summary>
        private void BtnDeletePatient_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPatients.SelectedItem == null)
            {
                MessageBox.Show("Выберите пациента для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selected = dataGridPatients.SelectedItem;
            int patientId = selected.Id;

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого пациента и всю его историю болезней?",
                                         "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using var context = new AppDbContext();
                    var patient = context.Patients.Find(patientId);

                    if (patient != null)
                    {
                        var records = context.MedicalRecords.Where(r => r.PatientId == patientId);
                        context.MedicalRecords.RemoveRange(records);

                        context.Patients.Remove(patient);
                        context.SaveChanges();
                    }

                    MessageBox.Show("Пациент удален.", "Успех");
                    LoadData(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
            }
        }

    }
}
