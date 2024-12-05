using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infrastructure.Data;

namespace Users.Infrastructure.Repositories
{
    public class OutboxMessageRepository(AppDbContext context) : IOutboxMessageRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(OutboxMessage outboxMessage)
        {
            await _context.OutboxMessages.AddAsync(outboxMessage);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<OutboxMessage>> GetPendingMessagesAsync() =>
            await _context.OutboxMessages.Where(msg => !msg.Processed).ToListAsync();

        public async Task<OutboxMessage?> GetByIdAsync(Guid outboxMessageId) =>
            await _context.OutboxMessages.FindAsync(outboxMessageId);

        public async Task MarkAsProcessedAsync(Guid outboxMessageId)
        {
            var message = await _context.OutboxMessages.FindAsync(outboxMessageId);

            if (message != null)
            {
                message.Processed = true;
                _context.OutboxMessages.Update(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}
