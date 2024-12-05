using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Data;
using Users.Infrastructure;
using Users.Domain.Interfaces;

namespace Users.Application.Processors
{
    public class OutboxMessageProcessor(AppDbContext context, ILogger<OutboxMessageProcessor> logger, IRabbitMQConnection rabbitMqConnection) : IHostedService
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<OutboxMessageProcessor> _logger = logger;
        // TODO: add interface
        private readonly IRabbitMQConnection _rabbitMqConnection = rabbitMqConnection;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await ProcessOutboxMessagesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task ProcessOutboxMessagesAsync()
        {
            var unprocessedMessages = await _context.OutboxMessages
                .Where(m => !m.Processed)
                .ToListAsync();

            var (exception, status) = await _rabbitMqConnection.Initialization;

            Console.WriteLine("exception" + exception);
            Console.WriteLine("status" + status);

            if (exception != null)
                throw exception;

            foreach (var message in unprocessedMessages)
            {
                try
                {
                    await _rabbitMqConnection.SendMessageAsync(
                        "sendUserIdToProject",
                        message.Payload
                    );

                    message.Processed = true;
                    _context.OutboxMessages.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar mensagem {message.OutboxMessageId}: {ex.Message}");
                }
            }

                    await _rabbitMqConnection.CloseAsync();
        }
    }
}
