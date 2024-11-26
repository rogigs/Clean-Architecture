using Auth.Controllers;
using Auth.Database.Entities;

namespace Auth.Database.Repositories
{
    public interface IAuthenticationRepository
    {
        Task Add(Authentication authentication);
        Task<Authentication?> Update(AuthenticationUpdateDTO authenticationUpdateDTO);
        Task<Authentication?> Delete(string email);
    }

    public class AuthenticationRepository(AppDbContext context) : IAuthenticationRepository
    {
        private readonly AppDbContext _context = context;

        public async Task Add(Authentication authentication)
        {
            await _context.Auth.AddAsync(authentication);
            await _context.SaveChangesAsync();
        }

        public async Task<Authentication?> Update(AuthenticationUpdateDTO authenticationUpdateDTO)
        {
            var authDB = await _context.Auth.FindAsync(authenticationUpdateDTO.Email);

            if (authDB == null) return null;

            authDB.Email = string.IsNullOrEmpty(authenticationUpdateDTO?.Email) ? authDB.Email : authenticationUpdateDTO.Email;
            authDB.Password = string.IsNullOrEmpty(authenticationUpdateDTO?.Password) ? authDB.Password : authenticationUpdateDTO.Password;

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
