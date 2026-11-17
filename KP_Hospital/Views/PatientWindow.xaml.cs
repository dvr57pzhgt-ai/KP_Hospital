using System;
using System.Windows;
using KP_Hospital.Data;
using KP_Hospital.Models;

namespace KP_Hospital.Views
{
    public partial class PatientWindow : Window
    {
        public PatientWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка на пустое ФИО
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО пациента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 2. Подключаемся к БД
                using var context = new AppDbContext();

                // 3. Создаем пациента
                var newPatient = new Patient
                {
                    FullName = txtFullName.Text.Trim(),
                    BirthDate = dpBirthDate.SelectedDate ?? DateTime.Now,
                    Gender = txtGender.Text.Trim(),
                    InsuranceNumber = txtInsurance.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim()
                };

                // 4. Сохраняем
                context.Patients.Add(newPatient);
                context.SaveChanges();

                MessageBox.Show("Пациент успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                // 5. Закрываем окно и говорим главному окну, что всё ок (true)
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}