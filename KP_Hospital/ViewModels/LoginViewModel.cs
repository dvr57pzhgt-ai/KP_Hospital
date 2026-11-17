using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KP_Hospital.Data;
using KP_Hospital.Services;
using System.Windows;

namespace KP_Hospital.ViewModels
{
    /// <summary>
    /// Модель представления для окна авторизации.
    /// </summary>
    public partial class LoginViewModel : ObservableObject
    {
        // Свойства (вручную, чтобы избежать ошибок генератора)
        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// Команда входа в систему.
        /// </summary>
      
        private void Login()
        {
            using var context = new AppDbContext();
            IAuthService authService = new AuthService(context);

            if (authService.Login(Username, Password))
            {
                // Открываем главное окно
                var mainWindow = new KP_Hospital.Views.MainWindow();
                mainWindow.Show();

                // Закрываем окно логина
                Application.Current.Windows[0].Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}