using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.Data;
using Users.Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Users.Application.Processors
{
    public class OutboxMessageProcessor(
        ILogger<OutboxMessageProcessor> logger,
        IServiceScopeFactory serviceScopeFactory) : IHostedService
    {
        private readonly ILogger<OutboxMessageProcessor> _logger = logger;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(() => ProcessOutboxMessagesAsync(cancellationToken), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var rabbitMqConnection = scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();

                    var unprocessedMessages = await context.OutboxMessages
                        .Where(m => !m.Processed)
                        .ToListAsync(cancellationToken);

                    var (exception, status) = await rabbitMqConnection.Initialization;

                    if (exception != null)
                    {
                        _logger.LogError("Error during RabbitMQ initialization: {Exception}", exception);
                        throw exception;
                    }

                    foreach (var message in unprocessedMessages)
                    {
                        try
                        {
                            await rabbitMqConnection.SendMessageAsync(
                                "sendUserIdToProject",
                                message.Payload
                            );

                            message.Processed = true;
                            context.OutboxMessages.Update(message);
                            await context.SaveChangesAsync(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error processing message {message.OutboxMessageId}: {ex.Message}");
                        }
                    }
                }

                await Task.Delay(1000, cancellationToken); // Delay to prevent busy-waiting
            }
        }
    }
}
