using System.Windows;
using Serilog;

namespace KP_Hospital
{
    public partial class App : Application
    {
        /// <summary>
        /// Точка входа в приложение. Настраивает логирование и запускает окно авторизации.
        /// </summary>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Запуск программы");

            var loginWindow = new KP_Hospital.Views.LoginWindow();
            loginWindow.Show();
        }

        /// <summary>
        /// Вызывается при закрытии приложения. Завершает логирование.
        /// </summary>
        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("Завершение работы программы");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}