using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KP_Hospital.Data;

namespace KP_Hospital.Services
{

        public class AuthService : IAuthService
        {
            private readonly AppDbContext _context;

            public AuthService(AppDbContext context)
            {
                _context = context;
            }

            public bool Login(string username, string password)
            {
                return _context.Doctors.Any(d => d.Login == username && d.PasswordHash == password);
            }
        }
    

}
