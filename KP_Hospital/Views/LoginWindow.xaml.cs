using System.Windows;
using System.Windows.Controls;
using KP_Hospital.Data;
using KP_Hospital.Services;

namespace KP_Hospital.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.LoginViewModel(); 
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