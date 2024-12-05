using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Users.Application.UseCases.DTO;
using System.Text.Json;

namespace Users.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context, IOutboxMessageRepository outboxMessageRepository) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IOutboxMessageRepository _outboxMessageRepository = outboxMessageRepository;

        public async Task<User?> GetById(Guid userId) => await _context.Users.FindAsync(userId);

        //TODO: change order by to createdAt
        public async Task<IEnumerable<User>> GetAll(PaginationDTO pagination) => await _context.Users.OrderBy(val => val.Name).Skip(pagination.Skip).Take(pagination.Take).ToListAsync();

        public async Task Add(User user, OutboxMessage outboxMessage)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                await _outboxMessageRepository.AddAsync(outboxMessage);

                await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();

                throw new ApplicationException("Error creating the user and recording the outbox message", ex);
            }
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
