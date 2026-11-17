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
            LoadData(); // Загружаем данные при открытии окна
        }

        /// <summary>
        /// Загружает всех пациентов из БД в таблицу вручную.
        /// </summary>
        private void LoadData()
        {
            try
            {
                using var context = new AppDbContext();

                // Получаем список пациентов
                var patients = context.Patients.ToList();

                // Очищаем таблицу
                dataGridPatients.Items.Clear();

                // Если пациентов нет, выводим сообщение (для отладки)
                if (patients.Count == 0)
                {
                    MessageBox.Show("В базе данных нет ни одного пациента!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Заполняем таблицу
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
            txtSearch.Text = string.Empty; // Очищаем поле поиска
            LoadData(); // Загружаем всех
        }

        /// <summary>
        /// Кнопка "Найти".
        /// </summary>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            using var context = new AppDbContext();

            // Ищем пациентов по ФИО
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

            // Если пациент добавлен, обновляем таблицу
            if (patientWindow.ShowDialog() == true)
            {
                LoadData();
            }
        }
    }
}