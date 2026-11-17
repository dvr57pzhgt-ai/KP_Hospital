using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_Hospital.Services
{

    /// <summary>
    /// Интерфейс для сервиса авторизации.
    /// Определяет контракт для проверки учетных данных врача.
    /// </summary>
    public interface IAuthService
    {
        bool Login(string username, string password);
    }
}
