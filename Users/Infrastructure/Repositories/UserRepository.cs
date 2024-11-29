using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Users.Application.UseCases.DTO;

namespace Users.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> GetById(Guid userId) => await _context.Users.FindAsync(userId);

        //TODO: change order by to createdAt
        public async Task<IEnumerable<User>> GetAll(PaginationDTO pagination) => await _context.Users.OrderBy(val => val.Name).Skip(pagination.Skip).Take(pagination.Take).ToListAsync();

        public async Task Add(User User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> Update(UserUpdateDTO userUpdateDTO, Guid userId)
        {
            var userDB = await _context.Users.FindAsync(userId);

            if (userDB == null) return null;

            userDB.Name = string.IsNullOrEmpty(userUpdateDTO?.Name) ? userDB.Name : userUpdateDTO.Name;
            userDB.Email = string.IsNullOrEmpty(userUpdateDTO?.Email) ? userDB.Email : userUpdateDTO.Email;

            _context.Users.Update(userDB);

            await _context.SaveChangesAsync();

            return userDB;
        }

        public async Task<User?> Delete(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;

        }
    }
}
