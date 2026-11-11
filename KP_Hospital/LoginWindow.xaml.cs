using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KP_Hospital.Data;
using KP_Hospital.Models;
using KP_Hospital.Services;


namespace KP_Hospital
{
        public partial class LoginWindow : Window
        {
            public LoginWindow()
            {
                InitializeComponent();
                InitializeDatabase();
            }

            private void InitializeDatabase()
            {
                using var context = new AppDbContext();
                context.Database.EnsureCreated(); 

               
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

            private void BtnLogin_Click(object sender, RoutedEventArgs e)
            {
                using var context = new AppDbContext();
                var authService = new AuthService(context);

                if (authService.Login(txtLogin.Text, txtPassword.Password))
                {
                    MessageBox.Show("Успешный вход!");
                 
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
}
