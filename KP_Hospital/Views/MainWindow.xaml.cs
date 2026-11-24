using System;
using System.Windows;
using System.Linq;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
        /// Кнопка "Найти".
        /// </summary>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            using var context = new AppDbContext();

           
            var patients = context.Patients
                .Where(p => p.FullName.Contains(searchText))
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
                    var recordWindow = new MedicalRecordWindow(patient);
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
        
    }
}
