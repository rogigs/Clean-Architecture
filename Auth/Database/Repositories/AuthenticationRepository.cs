﻿using Auth.Controllers;
using Auth.Database.Entities;
using Microsoft.EntityFrameworkCore;
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

        private static string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

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
            authDB.Password = isToChangePassword && ValidatePassword(authenticationUpdateDTO!.Password, authDB.Password)
                             ? HashPassword(authenticationUpdateDTO!.NewPassword)
                             : authDB.Password;
            //TODO: EF doesnt allow change PK
            authDB.Email = string.IsNullOrEmpty(authenticationUpdateDTO?.NewEmail) ? authDB.Email : authenticationUpdateDTO.NewEmail;

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
