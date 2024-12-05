using Users.Domain.Entities;

namespace Users.Domain.Interfaces
{
    public interface IOutboxMessageRepository
    {
        Task AddAsync(OutboxMessage outboxMessage);
        Task<IEnumerable<OutboxMessage>> GetPendingMessagesAsync();
        Task<OutboxMessage?> GetByIdAsync(Guid outboxMessageId);
        Task MarkAsProcessedAsync(Guid outboxMessageId);
    }
}
