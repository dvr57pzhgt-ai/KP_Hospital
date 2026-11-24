using System.Windows;
using System.Windows.Controls;
using KP_Hospital.Data;
using KP_Hospital.Models;
using KP_Hospital.Services;

namespace KP_Hospital.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.LoginViewModel();
            CreateTestUserIfNotExists();
        }

        private void CreateTestUserIfNotExists()
        {
            try
            {
                using var context = new AppDbContext();

                // Создаем базу данных (если её нет)
                context.Database.EnsureCreated();

                // Если в базе нет врачей — добавляем тестового
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
                MessageBox.Show($"Ошибка инициализации базы: {ex.Message}\n\n{ex.InnerException?.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using var context = new AppDbContext();
            IAuthService authService = new AuthService(context);

            var vm = DataContext as ViewModels.LoginViewModel;
            string username = vm?.Username ?? "";
            string password = txtPassword.Password;

            if (authService.Login(username, password))
            {
                var main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}