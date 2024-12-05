using RabbitMQ.Client;
using Users.Infrastructure.Exceptions;

namespace Users.Domain.Interfaces
{
    public interface IRabbitMQConnection : IAsyncInitializationRetry<RabbitMQException, string>
    {
        Task<(RabbitMQException?, string)> Initialization { get; }
        IChannel Channel { get; }
        IConnection Connection { get; }
        Task SendMessageAsync(string queueName, string message);
        Task<(RabbitMQException?, string)?> CloseAsync();
    }
}
