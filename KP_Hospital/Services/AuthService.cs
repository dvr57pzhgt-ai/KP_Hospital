using KP_Hospital.Data;

namespace KP_Hospital.Services
{
    /// <summary>
    /// Сервис для авторизации врачей в системе.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Проверяет наличие врача с указанным логином и паролем.
        /// </summary>

        public bool Login(string username, string password)
        {
            return _context.Doctors.Any(d => d.Login == username && d.PasswordHash == password);
        }
    }
}