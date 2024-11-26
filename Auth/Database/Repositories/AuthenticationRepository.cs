using Auth.Controllers;
using Auth.Database.Entities;
using System.Security.Authentication;

namespace Auth.Database.Repositories
{
    public interface IAuthenticationRepository
    {
        Task Add(Authentication authentication);
        Task<Authentication?> Update(AuthenticationUpdateDTO authenticationUpdateDTO);
        Task<Authentication?> Delete(string email);
        Task<Authentication?> GetByAuth(Authentication authentication);
    }

    public class AuthenticationRepository(AppDbContext context) : IAuthenticationRepository
    {
        private readonly AppDbContext _context = context;

        private static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

        private static bool ValidatePassword(string password, string storedHash) => BCrypt.Net.BCrypt.Verify(password, storedHash);

        public async Task Add(Authentication authentication)
        {
            authentication.Password = HashPassword(authentication.Password);
            await _context.Auth.AddAsync(authentication);
            await _context.SaveChangesAsync();
        }

        public async Task<Authentication?> GetByAuth(Authentication authentication)
        {
            var authDB = await _context.Auth.FindAsync(authentication.Email);

            if (!ValidatePassword(authentication!.Password, authDB!.Password)) throw new AuthenticationException("Invalid credentials.");

            return authDB;
        }

        public async Task<Authentication?> Update(AuthenticationUpdateDTO authenticationUpdateDTO)
        {
            var authDB = await _context.Auth.FindAsync(authenticationUpdateDTO.Email);

            if (authDB == null) return null;

            bool isToChangePassword = !string.IsNullOrEmpty(authenticationUpdateDTO?.Password) && !string.IsNullOrEmpty(authenticationUpdateDTO?.NewPassword);
            authDB.Password = isToChangePassword && ValidatePassword(authenticationUpdateDTO!.Password, authDB!.Password)
                             ? authenticationUpdateDTO!.NewPassword
                             : authDB.Password;
            authDB.Email = string.IsNullOrEmpty(authenticationUpdateDTO?.Email) ? authDB.Email : authenticationUpdateDTO.Email;

            _context.Auth.Update(authDB);

            await _context.SaveChangesAsync();

            return authDB;
        }

        public async Task<Authentication?> Delete(string email)
        {
            var auth = await _context.Auth.FindAsync(email);

            if (auth == null) return null;

            _context.Auth.Remove(auth);
            await _context.SaveChangesAsync();

            return auth;

        }
    }
}
