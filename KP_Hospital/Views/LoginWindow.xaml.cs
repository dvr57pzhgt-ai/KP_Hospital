using System;
using System.Linq;
using System.Windows;
using KP_Hospital.Data;
using KP_Hospital.Models;
using Microsoft.EntityFrameworkCore;

namespace KP_Hospital.Views
{
    /// <summary>
    /// Логика взаимодействия для окна авторизации.
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            CreateTestUserIfNotExists();
        }

        /// <summary>
        /// Создает тестового врача при первом запуске программы.
        /// </summary>
        private void CreateTestUserIfNotExists()
        {
            try
            {
                using var context = new AppDbContext();
                context.Database.Migrate();

                if (!context.Doctors.Any())
                {
                    context.Doctors.Add(new Doctor
                    {
                        FullName = "Иванов Иван Иванович",
                        Login = "admin",
                        PasswordHash = "123",
                        Specialization = "Терапевт",
                        Cabinet = "101",
                        Phone = "+7 (999) 123-45-67"
                    });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик кнопки "Войти".
        /// </summary>
        /// <summary>
        /// Обработчик кнопки "Войти". Проверяет логин и пароль врача.
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using var context = new AppDbContext();
                string loginInput = txtLogin.Text;
                string passwordInput = txtPassword.Password;

                var doctor = context.Doctors.FirstOrDefault(d => d.Login == loginInput && d.PasswordHash == passwordInput);

                if (doctor != null)
                {
                    var main = new MainWindow(doctor);
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка БД: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}